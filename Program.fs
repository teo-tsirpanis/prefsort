// This program was written by Theodore Tsirpanis in less than an hour.
// The author does bestow this program to the public domain, specifically under the CC0 license (https://creativecommons.org/publicdomain/zero/1.0/legalcode).

open Argu
open System
open System.IO

type PrefSortExiter() =
    interface IExiter with
        member __.Name = "paket exiter"
        member __.Exit (msg,code) =
            if code = ErrorCode.HelpText then
                eprintfn "%s" msg
                exit 0
            else
                eprintfn "%s" msg
                exit 1

type Arguments =
    | [<Unique; AltCommandLine("-i")>] InputFile of string
    | [<Unique; AltCommandLine("-o")>] OutputFile of string
    | [<Unique>] NoNumbers
    interface IArgParserTemplate with
        member x.Usage =
            match x with
            | InputFile _ -> "The file that contains the items to be sorted."
            | OutputFile _ -> "The file that will contain the sorted items. Writes to stdout if not specified."
            | NoNumbers -> "Numbers will not be prepended to the output."

let curry f x1 x2 = f(x1, x2)

let uncurry f (x1, x2) = f x1 x2

let rec prefSort x1 x2 =
    eprintf "Do You prefer %s (1), or %s (2)? Write your answer: " x1 x2
    match Console.ReadLine() with
    | "1" -> -1
    | "2" -> 1
    | _ ->
        eprintfn "Invalid answer. :-("
        prefSort x1 x2

let addNumbers =
    function
    | true -> Seq.indexed >> Seq.map (fun (c, s) -> sprintf "%d. %s" (c + 1) s)
    | false -> id

[<EntryPoint>]
let main argv =
    let parser = ArgumentParser.Create("prefsort", "Help requested: ", errorHandler = PrefSortExiter())
    let results = parser.ParseCommandLine()
    let inputFile = results.GetResult (<@ InputFile @>, defaultValue = "input.txt")
    let outputFile = results.GetResult (<@ OutputFile @>, defaultValue = "output.txt")
    let shouldAdd = results.Contains <@ NoNumbers @>

    inputFile
    |> File.ReadAllLines
    |> Seq.filter (String.IsNullOrWhiteSpace >> not)
    |> Seq.filter (Seq.head >> ((=) '#'))
    |> Seq.sortWith prefSort
    |> addNumbers shouldAdd
    |> curry File.WriteAllLines outputFile

    printfn "Success. Check out the results in %s." outputFile
    0

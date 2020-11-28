// This program was written by Theodore Tsirpanis in winter 2017.
// The author does bestow this program to the public domain,
// specifically under the CC0 license (https://creativecommons.org/publicdomain/zero/1.0/legalcode).

open Argu
open System
open System.Collections.Generic
open System.IO

type PrefSortExiter() =
    interface IExiter with
        member _.Name = "prefsort exiter"
        member _.Exit (msg,code) =
            if code = ErrorCode.HelpText then
                eprintfn "%s" msg
                exit 0
            else
                eprintfn "%s" msg
                exit 1

type Arguments =
    | [<Unique; MainCommand>] InputFile of string
    | [<Unique; AltCommandLine("-o")>] OutputFile of string
    | [<Unique>] NoNumbers
    interface IArgParserTemplate with
        member x.Usage =
            match x with
            | InputFile _ -> "The file that contains the items to be sorted."
            | OutputFile _ -> "The file that will contain the sorted items. Writes to stdout if not specified."
            | NoNumbers -> "Numbers will not be prepended to the output."

let rec makeMemoizablePrefSort () =
    let dict = Dictionary()
    let addDict ((x1, x2) as key) value =
        dict.Add (key, value)
        dict.Add ((x2, x1), -value)
        value
    let hasDict key =
        match dict.TryGetValue key with
        | (true, x) -> Some x
        | (false, _) -> None
    let rec prefSort x =
        match dict.TryGetValue x with
        | true, x -> x
        | false, _ ->
            x ||> eprintf "Do You prefer %s (1), or %s (2)? Write your answer: "
            match Console.ReadLine() with
            | "0" -> 0
            | "1" -> addDict x -1
            | "2" -> addDict x 1
            | _ ->
                eprintfn "Invalid answer. :-("
                prefSort x
    {new IComparer<string> with member _.Compare(x1, x2) = prefSort(x1, x2)}

let writeOutput noNumbers outFile (lines: string[]) =
    if noNumbers then
        File.WriteAllLines(outFile, lines)
    else
        use sr = new StreamWriter(outFile)
        let mutable i = 1
        for line in lines do
            sr.Write i
            i <- i + 1
            sr.Write ". "
            sr.WriteLine line

[<EntryPoint>]
let main _ =
    let parser = ArgumentParser.Create("prefsort", "Help requested: ", errorHandler = PrefSortExiter())
    let results = parser.ParseCommandLine()
    let inputFile = results.GetResult (InputFile)
    let outputFile = results.GetResult (OutputFile, defaultValue = Path.ChangeExtension(inputFile, "out.txt"))
    let noNumbers = results.Contains NoNumbers
    let comparer = makeMemoizablePrefSort()

    let lines =
        inputFile
        |> File.ReadLines
        |> Seq.filter (fun x -> not (String.IsNullOrWhiteSpace x || x.[0] = '#'))
        |> Array.ofSeq

    Array.Sort(lines, comparer)

    writeOutput noNumbers outputFile lines

    printfn "Success. Check out the results in %s." outputFile
    0

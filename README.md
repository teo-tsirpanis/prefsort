# prefsort

[![AppVeyor](https://img.shields.io/appveyor/ci/teo-tsirpanis/prefsort.svg?style=flat-square)](https://ci.appveyor.com/project/teo-tsirpanis/prefsort)

A program that sorts items, based on subjective criteria.

## What it is

Have you ever been in a situation where you have to rank things based on some criteria? And after you have done it, you realize that something is not right? This is so tedious to be done by a human. And that's where prefsort enters the game. It is great for making things like top-ten lists or videos.

## Usage guide for dummies 😛

1. [Download](https://github.com/teo-tsirpanis/prefsort/releases) prefsort and extract it to a folder.

> There are downloads for Windows, macOS and Linux. The `netcore` package is for those who have .NET Core 2.0 installed. It is smaller that the Linux and macOS ones, but is suited for more advances users. The Windows version requires the .NET Framework 4.5 which should be pre-installed in any modern system.

2. Open the file named `input.txt` and write at each line the items you want to rank.

__Example input file:__

```
One
Two
Three
Four
```

3. Run `prefsort.exe`. It will ask you, whether you prefer one item over another. If you have `n` items to rank, it will ask you about `n * log(n)` questions (log is the binary logarithm). You answer by typing `1` or `2`.

4. When it finishes, check out `output.txt`

__Example output file:__

```
1: One
2: Two
3: Three
4: Four
```

## How it works

prefsort just sorts the items with a custom comparison function that asks the user which item is "bigger".

## Advanced use

`prefsort --help`

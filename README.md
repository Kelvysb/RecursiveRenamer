# RecursiveRenamer
Command line tool to recursively rename files based on regex pattern search and replace.

Install:

```
dotnet tool install --global RecursiveRenamer
```
  * Requires .Net core 2.2 : https://dotnet.microsoft.com/download/dotnet-core/2.2
  * It may be nescessary to add the 'dotnet tools' folder to the path environment variable in windows.
  * in windows the dotnet tools folder it's '%USERPROFILE%\.dotnet\tools'
  
  
Usage:

```
Execute:
 --exec or -e <find pattern> [Replace string*]
     --path (optional defines the search path)
     --dir-filter <RegEx filter> (optional filter the directories to be searched)
     --file-filter <RegEx filter> (optional filter the files to be renamed)
     --save <pattern name> (optional saves pattern without running)
     --simulate or -sim (Executes a simulation of the renaming process)
     --complete or -comp (Shows the complete path on the rename report)
     <find pattern> special tags:
         <B> (Replace on the begining of the file name)
         <E> (Replace on the end of the file name, consider the extension)
         <e> (Replace on the end of the file name, not consider the extension)
     <Replace string> special tags:
         <S> (Numeric sequence begining from 1, for each folder.)
         <s> (Numeric sequence begining from 0, for each folder.)
    *(if omitted will consider empty string)

Run saved pattern:
 --run or -r <pattern name>
     --path (optional defines the search path)

List saved patterns:
 --list or -l

Get current dir:
 -env or --env

Program Version:
 -v or --version
Help:
 -h or --help
```

Examples:

Add sequence at the end of the file name, maintain the extension:
```
--exec <e> _<S>
```
Result:
filename.txt => filename_1.txt


Add prefix the file name:
```
--exec <B> prefix.
```
Result:
filename.txt => prefix.filename.txt


Remove prefix the file name:
```
--exec prefix\. ""
```
Result:
prefix.filename.txt => filename.txt


Save the pattern:
```
--exec prefix\. "" --save patternName
```


Run the saved pattern:
```
--run patternName
```




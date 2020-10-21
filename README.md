# PdfToolCli
[![NuGet version](https://badge.fury.io/nu/pdftoolcli.svg)](https://badge.fury.io/nu/pdftoolcli)

PdfToolCli is the command line tool that simply rotate and select pages in PDF documents.

This is .NET Core tool.

# How to use

## Rotate

### Rotate all page

Rotate all pages in target PDF document to specified angle.

#### Syntax
```
$ pdftool rotate all <PdfFilePath> <Angle>
```

|Argument|Description|
|--|--|
|```PdfFilePath```|Target PDF file path.|
|```Angle```|Rotate right: 90, 180, 270. Rotate left: -90, -180, -270.|

#### Options

|Otion|Function|
|--|--|
|```-o```, ```--output```|Output file path that you want to save to.|

#### Example

Rotate all pages to left 90° and save to 'output.pdf'.

```
$ pdftool rotate all MyPdf.pdf -90 -o output.pdf
```

### Rotate specified pages

Rotate specified pages in target PDF document to specified angle.

#### Syntax

```
$ pdftool rotate pages <PdfFilePath> <pageNum1:angle1,pageNum2:angle2,...>
```

|Argument|Description|
|--|--|
|```PdfFilePath```|Target PDF file path.|
|```pageNum:angle```|Rotate 'pageNum'-th page to 'angle'°.|

#### Options

|Otion|Function|
|--|--|
|```-o```, ```--output```|Output file path that you want to save to.|

#### Example

Rotate 1,2,5th page to right 180° and rotate 3rd page to left 90° and save to 'output.pdf'. 

```
$ pdftool rotate pages MyPdf.pdf 1:180,2:180,3:-90,5:180 -o output.pdf
```

or

```
$ pdftool rotate pages MyPdf.pdf 1-2:180,3:-90,5:180 -o output.pdf
```

## Select

### Select specified pages

Select and save specified page in target PDF document.

### Syntax

```
$ pdftool select pages <PdfFilePath> <pageNum1,pageNum2,...>
```

|Argument|Description|
|--|--|
|```PdfFilePath```|Target PDF file path.|
|```pageNum```|Page number that you want to select.|

#### Options

|Otion|Function|
|--|--|
|```-o```, ```--output```|Output file path that you want to save to.|

#### Example

Select 1,2,3,5th page and save to 'output.pdf'.

```
$ pdftool select pages MyPdf.pdf 1,2,3,5 -o output.pdf
```

or

```
$ pdftool select pages MyPdf.pdf 1-3,5 -o output.pdf
```


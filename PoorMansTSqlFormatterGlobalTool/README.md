# Poor Man's T-SQL Formatter Global Tool

Install Poor Man's T-SQL Formatter as a [.NET Core global tool](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools)

## Manage

### Bulid

```
> dotnet pack
```

### Install

```
> dotnet tool install --global --add-source ./nupkg PoorMansTSqlFormatterGlobalTool
```

### Update

```
> dotnet tool update --global --add-source ./nupkg PoorMansTSqlFormatterGlobalTool
```

### Uninstall

```
> dotnet tool uninstall --global PoorMansTSqlFormatterGlobalTool
```

## Usage

```
> poorsql --help

Poor Man's T-SQL Formatter - a small free Transact-SQL formatting
library for .Net 2.0 and JS, written in C#. Distributed under AGPL v3.
Copyright (C) 2011-2017 Tao Klerks


Usage: poorsql [options] <FILE>

Arguments:
  FILE                                           One or many files to be formatted. With no FILE, read standard input

Options:
  --indent-string <INDENT_STRING>                Indent string. Default is '\t'
  --spaces-per-tab <SPACES_PER_TAB>              Spaces per tab. Default is 4
  --max-line-width <MAX_LINE_WIDTH>              Max line width. Default is 999
  --trailing-commas                              Trailing commas
  --space-after-expanded-comma                   Space after expanded comma
  --disable-expand-comma-lists                   Disable expand comma lists
  --disable-expand-boolean-expressions           Disable expand boolean expressions
  --disable-expand-between-conditions            Disable expand between conditions
  --disable-expand-case-statements               Disable expand CASE statements
  --disable-expand-in-lists                      Disable expand IN lists
  --disable-uppercase-keywords                   Disable uppercase keywords
  --break-join-on-sections                       Break join on sections
  --html-coloring                                HTML coloring
  --keyword-standardization                      Keyword standardization
  --clause-break <NEW_CLAUSE_LINE_BREAKS>        New clause line breaks. Default is 1
  --statement-break <NEW_STATEMENT_LINE_BREAKS>  New statement line breaks. Default is 2
  --allow-parsing-errors                         Allow parsing errors
  -?|-h|--help                                   Show help information
```

## Example

### Format T-SQL in Vim

Add the following to `.vimrc` to format T-SQL in vim by hitting `<F10>`

```
nnoremap <F10> m`gg=G``
autocmd FileType sql setlocal equalprg=poorsql\ --trailing-commas
```

using System;
using System.IO;
using PoorMansTSqlFormatterLib;
using PoorMansTSqlFormatterLib.Formatters;
using McMaster.Extensions.CommandLineUtils;
using System.Text;

namespace PoorMansTSqlFormatterGlobalTool
{
	[Command(
		Name = "poorsql",
		Description = @"
Poor Man's T-SQL Formatter - a small free Transact-SQL formatting 
library for .Net 2.0 and JS, written in C#. Distributed under AGPL v3.
Copyright (C) 2011-2017 Tao Klerks
",

		ExtendedHelpText = @"
"
	)]
	class Program
	{
		private SqlFormattingManager _formattingManager;

		// --- 

		[Option(ShortName = "", Description = "Indent string. Default is '\\t'")]
		public string IndentString { get; } = "\t";

		[Option(ShortName = "", Description = "Spaces per tab. Default is 4")]
		public int SpacesPerTab { get; } = 4;

		[Option(ShortName = "", Description = "Max line width. Default is 999")]
		public int MaxLineWidth { get; } = 999;

		[Option(ShortName = "", Description = "Trailing commas")]
		public bool TrailingCommas { get; } = false;

		[Option(ShortName = "", Description = "Space after expanded comma")]
		public bool SpaceAfterExpandedComma { get; } = false;

		[Option(ShortName = "", LongName = "disable-expand-comma-lists", Description = "Disable expand comma lists")]
		public bool DisableExpandCommaLists { get; } = false;
		public bool ExpandCommaLists => !DisableExpandCommaLists;

		[Option(ShortName = "", LongName = "disable-expand-boolean-expressions", Description = "Disable expand boolean expressions")]
		public bool DisableExpandBooleanExpressions { get; } = false;
		public bool ExpandBooleanExpressions => !DisableExpandBooleanExpressions;

		[Option(ShortName = "", LongName = "disable-expand-between-conditions", Description = "Disable expand between conditions")]
		public bool DisableExpandBetweenConditions { get; } = false;
		public bool ExpandBetweenConditions => !DisableExpandBetweenConditions;

		[Option(ShortName = "", LongName = "disable-expand-case-statements", Description = "Disable expand CASE statements")]
		public bool DisableExpandCaseStatements { get; } = false;
		public bool ExpandCaseStatements => !DisableExpandCaseStatements;

		[Option(ShortName = "", LongName = "disable-expand-in-lists", Description = "Disable expand IN lists")]
		public bool DisableExpandInLists { get; } = false;
		public bool ExpandInLists => !DisableExpandInLists;

		[Option(ShortName = "", LongName = "disable-uppercase-keywords", Description = "Disable uppercase keywords")]
		public bool DisableUppercaseKeywords { get; } = false;
		public bool UppercaseKeywords => !DisableUppercaseKeywords;

		[Option(ShortName = "", Description = "Break join on sections")]
		public bool BreakJoinOnSections { get; } = false;

		[Option(ShortName = "", Description = "HTML coloring")]
		public bool HtmlColoring { get; } = false;

		[Option(ShortName = "", Description = "Keyword standardization")]
		public bool KeywordStandardization { get; } = false;

		[Option(ShortName = "", LongName = "clause-break", Description = "New clause line breaks. Default is 1")]
		public int NewClauseLineBreaks { get; } = 1;

		[Option(ShortName = "", LongName = "statement-break", Description = "New statement line breaks. Default is 2")]
		public int NewStatementLineBreaks { get; } = 2;

		[Option(ShortName = "", Description = "Allow parsing errors")]
		public bool AllowParsingErrors { get; } = false;

		[Argument(0, "FILE", "One or many files to be formatted. With no FILE, read standard input")]
		[LegalFilePath]
		public string[] Files { get; }

		// --- 

		public static int Main(string[] args) =>
			CommandLineApplication.Execute<Program>(args);

		private int OnExecute(CommandLineApplication commandLineApplication)
		{
			if (Console.IsInputRedirected)
			{
				Console.InputEncoding = Encoding.UTF8;
				var input = Console.In.ReadToEnd();
				var formattedOutput = Format(input);
				Console.WriteLine(formattedOutput);
				return 0;
			}
			else if (Files != null)
			{
				foreach (var file in Files)
				{
					if (File.Exists(file))
					{
						var input = File.ReadAllText(file);
						var formattedOutput = Format(input);
						Console.WriteLine(formattedOutput);
					}
				}
				return 0;
			}
			else
			{
				Console.Error.WriteLine(commandLineApplication.GetHelpText());
				return 1;
			}
		}


		private string Format(string inputSql)
		{
			if (_formattingManager == null)
			{
				var options = new TSqlStandardFormatterOptions
				{
					IndentString = IndentString,
					SpacesPerTab = SpacesPerTab,
					MaxLineWidth = MaxLineWidth,
					ExpandCommaLists = ExpandCommaLists,
					TrailingCommas = TrailingCommas,
					SpaceAfterExpandedComma = SpaceAfterExpandedComma,
					ExpandBooleanExpressions = ExpandBooleanExpressions,
					ExpandBetweenConditions = ExpandBetweenConditions,
					ExpandCaseStatements = ExpandCaseStatements,
					UppercaseKeywords = UppercaseKeywords,
					BreakJoinOnSections = BreakJoinOnSections,
					HTMLColoring = HtmlColoring,
					KeywordStandardization = KeywordStandardization,
					ExpandInLists = ExpandInLists,
					NewClauseLineBreaks = NewClauseLineBreaks,
					NewStatementLineBreaks = NewStatementLineBreaks,
				};

				var formatter = new TSqlStandardFormatter(options)
				{
					ErrorOutputPrefix = "--WARNING! ERRORS ENCOUNTERED DURING SQL PARSING!" + Environment.NewLine
				};

				_formattingManager = new SqlFormattingManager(formatter);
			}

			var errorEncountered = false;
			var formattedOutput = _formattingManager.Format(inputSql, ref errorEncountered);

			errorEncountered = !AllowParsingErrors && errorEncountered;
			if (errorEncountered)
			{
				throw new Exception("Error Encountered: {formattedOutput}");
			}

			return formattedOutput;
		}

	}
}

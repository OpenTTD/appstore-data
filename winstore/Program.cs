using CsvHelper;
using System.Globalization;
using System.Text;

namespace convert_to_winstore
{
	/// <summary>
	/// Class representing an OpenTTD language.
	/// </summary>
	internal class Language
	{
		/// <summary>
		/// Language filename base (e.g. "english");
		/// </summary>
		public string LangFile { get; }

		/// <summary>
		/// Windows Store language code (e.g. "en-gb");
		/// </summary>
		public string StoreCode { get; }

		/// <summary>
		/// Loaded strings for substitution.
		/// </summary>
		private Dictionary<string, string> strings;

		/// <summary>
		/// Creates a new instance of the Language class.
		/// </summary>
		/// <param name="langFile">Language filename base.</param>
		/// <param name="storeCode">Windows Store language code.</param>
		public Language(string langFile, string storeCode)
		{
			LangFile = langFile;
			StoreCode = storeCode;
		}

		/// <summary>
		/// Replaces $VARIABLES$ within the specified string with translated strings.
		/// </summary>
		/// <param name="langPath">Path to the 'lang' folder.</param>
		/// <param name="stringToSubstitute">String to work on.</param>
		/// <returns>The substituted string.</returns>
		public string Substitute(string langPath, string stringToSubstitute)
		{
			if (strings == null)
			{
				strings = new Dictionary<string, string>(StringComparer.Ordinal);
				string path = Path.Combine(langPath, Path.ChangeExtension(LangFile, ".txt"));

				if (!File.Exists(path))
				{
					throw new FileNotFoundException($"The '{LangFile}' file is missing.");
				}

				using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
					{
						while (!sr.EndOfStream)
						{
							string? line = sr.ReadLine();

							if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
							{
								continue;
							}

							int firstColon = line.IndexOf(':');

							if (firstColon < 0)
							{
								continue;
							}

							string key = $"${line.Substring(0, firstColon).Trim()}$";
							string value = line.Substring(firstColon + 1).Trim();

							strings[key] = value;
						}
					}
				}
			}

			foreach (KeyValuePair<string, string> kvp in strings)
			{
				stringToSubstitute = stringToSubstitute.Replace(kvp.Key, kvp.Value, StringComparison.Ordinal);
			}

			return stringToSubstitute;
		}
	}

	internal class Program
	{
		/// <summary>
		/// List of supported languages.
		/// </summary>
		private static readonly List<Language> languageMappings =
		[
			new Language("afrikaans", "af-za"),
			new Language("arabic_egypt", "ar-eg"),
			new Language("basque", "eu-es"),
			new Language("belarusian", "be-by"),
			new Language("brazilian_portuguese", "pt-br"),
			new Language("bulgarian", "bg-bg"),
			new Language("bulgarian", "bg-bg"),
			new Language("catalan", "ca-es"),
			new Language("croatian", "hr-hr"),
			new Language("czech", "cs-cz"),
			new Language("danish", "da-dk"),
			new Language("dutch", "nl-nl"),
			new Language("estonian", "et-ee"),
			new Language("finnish", "fi-fi"),
			new Language("french", "fr-fr"),
			new Language("gaelic", "gd-gb"),
			new Language("galician", "gl-es"),
			new Language("german", "de-de"),
			new Language("greek", "el-gr"),
			new Language("hebrew", "he-il"),
			new Language("hungarian", "hu-hu"),
			new Language("icelandic", "is-is"),
			new Language("indonesian", "id-id"),
			new Language("irish", "ga-ie"),
			new Language("italian", "it-it"),
			new Language("japanese", "ja-jp"),
			new Language("korean", "ko-kr"),
			new Language("latvian", "lv-lv"),
			new Language("lithuanian", "lt-lt"),
			new Language("luxembourgish", "lb-lu"),
			new Language("malay", "ms-my"),
			new Language("norwegian_bokmal", "nb-no"),
			new Language("norwegian_nynorsk", "no-no"),
			new Language("polish", "pl-pl"),
			new Language("portuguese", "pt-pt"),
			new Language("romanian", "ro-ro"),
			new Language("russian", "ru-ru"),
			new Language("serbian", "sr-latn-rs"),
			new Language("simplified_chinese", "zh-cn"),
			new Language("slovak", "sk-sk"),
			new Language("slovenian", "sl-si"),
			new Language("spanish", "es-es"),
			new Language("spanish_MX", "es-mx"),
			new Language("swedish", "sv-se"),
			new Language("tamil", "ta-in"),
			new Language("thai", "th-th"),
			new Language("traditional_chinese", "zh-tw"),
			new Language("turkish", "tr-tr"),
			new Language("ukrainian", "uk-ua"),
			new Language("vietnamese", "vi-vn"),
			new Language("welsh", "cy-gb")
		];

		/// <summary>
		/// English language variants (except "en-gb", which is included in the template).
		/// </summary>
		private static readonly string[] languageEnglishVariants =
		{
			"en-au",
			"en-us"
		};

		/// <summary>
		/// The English (UK) base translation.
		/// </summary>
		private static readonly Language englishGb = new Language("english", "en-gb");

		static void Main(string[] args)
		{
			// Either search the current directory for our data, or allow the path to be passed on the command line
			string rootPath = (args.Length > 0) ? args[0] : Directory.GetCurrentDirectory();

			string templatePath = Path.Combine(rootPath, "winstore-template.csv");
			string releaseNotesPath = Path.Combine(rootPath, "winstore-releasenotes.txt");
			string langPath = Path.Combine(rootPath, "lang");
			string outputPath = Path.Combine(rootPath, "winstore-output.csv");

			using (StreamWriter writer = new StreamWriter(outputPath, false, Encoding.UTF8))
			{
				using (CsvWriter csvOut = new CsvWriter(writer, new CultureInfo("en-US")))
				{
					using (StreamReader reader = new StreamReader(templatePath, true))
					{
						using (CsvReader csv = new CsvReader(reader, new CultureInfo("en-US")))
						{
							// Read the header row
							csv.Read();

							if (!csv.ReadHeader() || csv.HeaderRecord == null)
							{
								Console.Error.WriteLine("Unable to read header from input CSV.");
								return;
							}

							// Create output headers - one for each language
							string[] headers = new string[csv.HeaderRecord.Length + languageMappings.Count + languageEnglishVariants.Length];

							csv.HeaderRecord.CopyTo(headers, 0);
							languageMappings.Select(l => l.StoreCode).ToArray().CopyTo(headers, csv.HeaderRecord.Length);
							languageEnglishVariants.CopyTo(headers, csv.HeaderRecord.Length + languageMappings.Count);

							foreach (string value in headers)
							{
								csvOut.WriteField(value);
							}

							csvOut.NextRecord();

							string[] values = (string[])headers.Clone();

							while (csv.Read())
							{
								string fieldName = csv.GetField(0) ?? string.Empty;
								string fieldId = csv.GetField(1) ?? string.Empty;
								string fieldType = csv.GetField(2) ?? string.Empty;
								string fieldDefault = csv.GetField(3) ?? string.Empty;
								string enGbValue = csv.GetField(4) ?? string.Empty;

								values[0] = fieldName;
								values[1] = fieldId;
								values[2] = fieldType;
								values[3] = fieldDefault;

								for (int i = 4; i < headers.Length; i++)
								{
									values[i] = enGbValue;
								}

								if (fieldType.Equals("Text", StringComparison.Ordinal) && enGbValue.Contains('$', StringComparison.Ordinal))
								{
									if (enGbValue.Equals("$RELEASE_NOTES$", StringComparison.Ordinal))
									{
										// Read the release notes from a text file
										values[4] = enGbValue = File.ReadAllText(releaseNotesPath);

										if (values[4].Length > 1500)
										{
											Console.WriteLine("Warning: release notes exceed 1500 character limit ({0} characters).", values[4].Length);
										}
									}
									else if (enGbValue.Contains("$YEAR$", StringComparison.Ordinal))
									{
										// Replace $YEAR$ with the current year
										values[4] = enGbValue = enGbValue.Replace("$YEAR$", DateTime.UtcNow.Year.ToString());
									}
									else
									{
										// Perform a language lookup and substitution
										values[4] = englishGb.Substitute(langPath, values[4]);
									}

									int curCol = 5;

									foreach (Language l in languageMappings)
									{
										values[curCol] = l.Substitute(langPath, enGbValue);

										// If the string still contains unsubstituted values, use the English version instead
										if (values[curCol].Contains('$'))
											values[curCol] = values[4];

										curCol++;
									}

									// Manually set the English variants to the same as British English
									foreach (string variant in languageEnglishVariants)
									{
										values[curCol] = values[4];
										curCol++;
									}
								}

								foreach (string value in values)
								{
									csvOut.WriteField(value);
								}

								csvOut.NextRecord();
							}
						}
					}
				}
			}
		}
	}
}

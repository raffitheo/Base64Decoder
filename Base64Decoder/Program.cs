/*---------------------------------------
 * Author: Raffaele Valenti
 * Copyright © 2018-2020 Raffaele Valenti
---------------------------------------*/
#region Definitions
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
#endregion

namespace Base64Decoder
{
    #region Porgram class
	/// <summary>
	/// Main program class.
	/// </summary>
    class Program
	{
        #region Program's classes
        static void Main(string[] args)
		{
			List<string> fileLines = new List<string>();
			bool error = false;

			Console.Title = "Base64 Decoder 1.1.5.0";

			Console.Write("Base64 ");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("Decoder\n");
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write("Copyright <C> 2018-2020 Raffaele Valenti\n");

			bool fileFound = false;
			string file = null;
			do
			{
				file = ReadInputString("\nPlease, enter the file to decode:");

				if (file.Equals(@"\help"))
				{
					Console.WriteLine("File path example: 'C:\\Users\\$username$\\Desktop\\$filewithextension$'.");
					Console.WriteLine("Type '\\exit' to exit the application.");
				}
				else if (file.Equals(@"\exit")) { break; }
				else
				{
					if (File.Exists(file)) { fileFound = true; }
					else
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("[ERROR] This file was not found!\nType '\\help' to get some help.");
						Console.ForegroundColor = ConsoleColor.Gray;
					}
				}
			} while (!fileFound);

			if (fileFound)
			{
				Console.WriteLine("\nDecoding file form Base64...");

				string[] lines;
				using (StreamReader settingReader = new StreamReader(file, Encoding.Default))
				{
					lines = File.ReadAllLines(file);
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("[DEBUG] {lines.Length} lines were found inside this file.");
					Console.ForegroundColor = ConsoleColor.Gray;

					float readPercentage = 0;
					for (int i = 0; i < lines.Length; i++)
					{
						readPercentage += (float)100 / (float)lines.Length;
						Console.Write($"\rEncoding file: {(int)readPercentage}%");

						try
						{
							if (lines[i].StartsWith("/*") && lines[i].EndsWith("*/")) { fileLines.Add(lines[i]); }
							else
							{
								byte[] decodedBytes = Convert.FromBase64String(lines[i]);
								fileLines.Add(Encoding.UTF8.GetString(decodedBytes));
							}
						}
						catch
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine($"[ERROR] The line number {(i + 1)} can not be decoded!");
							Console.ForegroundColor = ConsoleColor.Gray;
							error = true;
							break;
						}
						Console.WriteLine();
					}
				}

				if (!error)
				{
					bool optionPositive = false;
					string newFile = null;
					do
					{
						newFile = ReadInputString("\nPlease, enter the name of the decoded file:");

						if (File.Exists(newFile))
						{
							bool optionFound = false;
							do
							{
								Console.ForegroundColor = ConsoleColor.Yellow;
								Console.WriteLine("[WARNING] This file already exists, do you want to override it? (Y/N)");
								Console.ForegroundColor = ConsoleColor.Gray;
								var option = Console.ReadLine();

								if (option.Equals("Y") || option.Equals("y") || option.Equals("N") || option.Equals("n"))
								{
									optionFound = true;
									if (option.Equals("Y") || option.Equals("y")) { optionPositive = true; }
								}
								else
								{
									Console.ForegroundColor = ConsoleColor.Red;
									Console.WriteLine("[ERROR] This input is not permitted!");
									Console.ForegroundColor = ConsoleColor.Gray;
								}
							} while (!optionFound);

							if (optionPositive) { File.Delete(newFile); }
						}
						else { optionPositive = true; }

						if (optionPositive)
						{
							File.Create(newFile).Dispose();
							using (StreamWriter fileWriter = new StreamWriter(newFile, true))
							{
								float writePercentage = 0;
								foreach (string line in fileLines)
								{
									writePercentage += (float)100 / (float)fileLines.Count;
									Console.Write($"\rWriting file: {(int)writePercentage}%");
									fileWriter.WriteLine(line);
									Console.WriteLine();
								}
							}
						}
					} while (!optionPositive);
				}

				Console.WriteLine("\nPress any key to exit...");
				Console.Read();
			}
		}
        #endregion

        #region Custom classes
        /// <summary>
        /// Read a console input string.
        /// </summary>
        /// <param name="_text">The text to display before the input.</param>
        /// <returns></returns>
        private static string ReadInputString(string _text)
		{
			Console.WriteLine(_text);
			var readLine = Console.ReadLine();
			return readLine.ToString();
		}
        #endregion
    }
    #endregion
}

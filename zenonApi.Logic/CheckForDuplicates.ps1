# Checks for duplicate zenonSerializableNodes, -Attributes, -Enums, -etc. in the current folder.
# This is crucial, since otherwise exceptions occur when trying to serialize/deserialize an object.

$code = @"
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;

    namespace zenonApi
    {
        public static class Helper
        {
            public static void CheckForDuplicates(string folder)
            {
                if (folder == null){
                    throw new Exception("No folder was specified");
                }

                Console.WriteLine("Checking for duplicates in " + folder + " and subfolders...");
                var files = Directory.EnumerateFiles(folder, "*.cs", SearchOption.AllDirectories);
                
                var regex = new Regex(@"\[zenonSerializable(\w+)\s*\(\s*(?:\`"`"|nameof\()([^,\`"`"\)]+)");
                int duplicates = 0;

                foreach (var file in files){
                    string content = File.ReadAllText(file);
                    
                    List<string> found = new List<string>();
                    foreach (Match match in regex.Matches(content)){
                        if (!found.Contains(match.Value)){
                            found.Add(match.Value);
                        }
                        else{
                            Console.WriteLine("Duplicate " + match.Groups[1] + " found in " + Path.GetFileName(file) + ": " + match.Groups[2]);
                            duplicates++;
                        }
                    }
                }

                if (duplicates != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("Found " + duplicates + " duplicates.");
                    Console.ResetColor();
                    Environment.Exit(-1);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("All good, no duplicates found.");
                    Console.ResetColor();
                }
            }
        }
    }
"@

try
{
   [zenonApi.Helper] -is [type];
}
catch
{
    Add-Type -TypeDefinition $code -Language CSharp;
}

$folder = Get-Location;
[zenonApi.Helper]::CheckForDuplicates($folder);
using System;
using SoulsFormats;
using FileHandling;
using InputHandling;
using PatchComparsion;

namespace SekiroParamCompare;

class Program
{
    static void Main(string[] args)
    {
        FileHandler fileHandler = new();
        InputHandler inputHandler = new();
        PatchCompare patchCompare = new();

        List<string> ignoredParam = new List<string>{@"\default_EnemyBehaviorBank.param", @"\default_AIStandardInfoBank.param", @"\LodParam.param", @"\LodParam_ps4.param", @"\LodParam_xb1.param", @"\DefaultKeyAssignParam04.param", @"\DefaultKeyAssignParam03.param", @"\DefaultKeyAssignParam02.param", @"\DefaultKeyAssignParam01.param", @"\DefaultKeyAssignParam00.param"};

        // Get version numbers to compare
        var (ver1, ver2) = inputHandler.GetVersionNumber();
        // Create output file
        fileHandler.CreateOutputFile(@$".\Out\1_0_{ver1}_to_1_0_{ver2}.txt");
        // Initialize string to write output to - we don't want to constantly write to the file
        string shit2output = "";

        // Recognize which patch is newer
        var (newPatch, oldPatch) = patchCompare.GetNewerPatch(ver1, ver2);
        

        // Loop does the checking - I have to refactor this mess...
        foreach (var file in newPatch.Files)
        {
            string baseFileName = file.Name.Replace(@"N:\NTC\data\Target\INTERROOT_win64\param\GameParam", "");
            if (ignoredParam.Contains(baseFileName))
                continue;
            Console.WriteLine($"Processing: {baseFileName.Replace(".param", "").Replace(@"\", "")}");
            shit2output += $"\nProcessing: {baseFileName.Replace(".param", "").Replace(@"\", "")}";
            PARAM newParamCurrent = PARAM.Read(file.Bytes);

            // Disable warning - yes the param COULD not exist, but it will unless the user changes something to gameparams in Assets folder
            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            PARAM oldParamCurrent = PARAM.Read(oldPatch.Files.Find(x => x.Name == @$"N:\NTC\data\Target\INTERROOT_win64\param\GameParam{baseFileName}").Bytes);

            foreach (var row in newParamCurrent.Rows)
            {
                if (oldParamCurrent.Rows.FindAll(x => x.ID == row.ID).Count < 1)
                {
                    Console.WriteLine($"{row.ID} - added");
                    shit2output += $"{row.ID} - added\n";
                    continue;
                }
                newParamCurrent.ApplyParamdef(PARAMDEF.XmlDeserialize(@$".\Assets\paramdef\{baseFileName.Replace(".param", "").Replace(@"\", "")}.xml"));
                oldParamCurrent.ApplyParamdef(PARAMDEF.XmlDeserialize(@$".\Assets\paramdef\{baseFileName.Replace(".param", "").Replace(@"\", "")}.xml"));

                for (int i = 0; i < row.Cells.Count; i++)
                {
                    var rowOld = oldParamCurrent.Rows.Find(x => x.ID == row.ID);
                    if (row.Cells[i].Value.ToString() != rowOld.Cells[i].Value.ToString())
                    {
                        Console.WriteLine($"{row.ID} - {row.Cells[i].InternalName} value changed from {row.Cells[i].Value} to {rowOld.Cells[i].Value}");
                        shit2output += $"{row.ID} - {row.Cells[i].InternalName} value changed from {row.Cells[i].Value} to {rowOld.Cells[i].Value}\n";
                    }
                }
            }
            #pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
        File.WriteAllText(@$".\Out\1_0_{ver1}_to_1_0_{ver2}.txt", shit2output);
    }
}
using System;
using SoulsFormats;

namespace FileHandling;

class FileHandler
{
    public BND4 ReadBND(string path)
    {
        return BND4.Read(@$".\Assets\gameparam\{path}");
    }

    public void ApplyAllParamdef(BND4 data)
    {
        foreach (var file in data.Files)
        {
            string baseName = file.Name.Replace(@"N:\NTC\data\Target\INTERROOT_win64\param\GameParam", "");
            List<string> ignoredParam = new List<string>{@"\default_EnemyBehaviorBank.param", @"\default_AIStandardInfoBank.param", @"\LodParam.param", @"\LodParam_ps4.param", @"\LodParam_xb1.param"};
            try
            {
                PARAM.Read(file.Bytes).ApplyParamdef(PARAMDEF.XmlDeserialize(@$".\Assets\paramdef\{baseName.Replace(".param", "")}.xml"));
            }
            catch (FileNotFoundException)
            {
                if (!ignoredParam.Contains(baseName))
                    Console.WriteLine($"Paramdef not found for {baseName}");
            }
        }
    }

    public List<PARAM> GetAllParamAsList(BND4 data)
    {
        List<PARAM> output = new();
        foreach (var file in data.Files)
        {
            output.Add(PARAM.Read(file.Bytes));
        }
        return output;
    }

    public void CreateOutputFile(string outdir, string file)
    {
        Directory.CreateDirectory(@$".\{outdir}");
        File.WriteAllText(@$".\{outdir}\{file}", "");
    }
}
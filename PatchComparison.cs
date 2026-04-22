using SoulsFormats;

namespace PatchComparsion;

class PatchCompare
{
    public (BND4, BND4) GetNewerPatch(int ver1, int ver2)
    {
        BND4 newPatch, oldPatch;
        if (ver1 > ver2)
        {
            newPatch = BND4.Read(@$".\Assets\gameparam\param_1_0_{ver1}.dcx");
            oldPatch = BND4.Read(@$".\Assets\gameparam\param_1_0_{ver2}.dcx");
        }
        else
        {
            newPatch = BND4.Read(@$".\Assets\gameparam\param_1_0_{ver2}.dcx");
            oldPatch = BND4.Read(@$".\Assets\gameparam\param_1_0_{ver1}.dcx");
        }
        return (newPatch, oldPatch);
    }
}
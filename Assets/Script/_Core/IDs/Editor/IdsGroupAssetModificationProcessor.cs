using System.Collections;
using UnityEditor;
using UnityEngine;
using System.IO;

public class IdsGroupAssetModificationProcessor : UnityEditor.AssetModificationProcessor
{
    private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
    {
        IdsGroup idsGroup = AssetDatabase.LoadMainAssetAtPath(sourcePath) as IdsGroup;

        if (idsGroup == null)
        {
            return AssetMoveResult.DidNotMove;
        }

        string srcDir = Path.GetDirectoryName(sourcePath);
        string dstDir = Path.GetDirectoryName(destinationPath);

        if (srcDir != dstDir)
        {
            return AssetMoveResult.DidNotMove;
        }

        string fileName = Path.GetFileNameWithoutExtension(destinationPath);
        idsGroup.name = fileName;

        return AssetMoveResult.DidNotMove;
    }
}

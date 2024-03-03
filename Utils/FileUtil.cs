using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PolicyManager.Utils;

public static partial class FileUtil
{
    [LibraryImport("shell32.dll")]
    // ReSharper disable once InconsistentNaming
    private static partial int SHOpenFolderAndSelectItems(IntPtr pidlFolder, uint cidl, [In] [MarshalAs(UnmanagedType.LPArray)] IntPtr[] apidl, uint dwFlags);

    [LibraryImport("shell32.dll", StringMarshalling = StringMarshalling.Utf16)]
    // ReSharper disable once InconsistentNaming
    private static partial IntPtr ILCreateFromPath([MarshalAs(UnmanagedType.LPWStr)] string pszPath);

    public static bool OpenFolder(string folderPath, List<string> selectFiles = null)
    {
        var dir = ILCreateFromPath(folderPath);

        if (selectFiles == null || selectFiles.Count == 0)
        {
            var result = SHOpenFolderAndSelectItems(dir, 0, null, 0);
            return result == 0;
        }
        else
        {
            var filesToSelectIntPtrArr = new IntPtr[selectFiles.Count];

            for (var i = 0; i < selectFiles.Count; i++)
            {
                filesToSelectIntPtrArr[i] = ILCreateFromPath(selectFiles[i]);
            }

            var result = SHOpenFolderAndSelectItems(dir, (uint)selectFiles.Count, filesToSelectIntPtrArr, 0);
            return result == 0;
        }
    }
}
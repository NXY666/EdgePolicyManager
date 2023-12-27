using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;
using Windows.Win32.UI.Shell;
using Windows.Win32.UI.Shell.Common;
using WinRT.Interop;

namespace PolicyManager.Utils;

public class SaveFilePicker
{
    private string _title = ResourceUtil.GetString("SaveFilePicker/Title");
    private string _fileName = ResourceUtil.GetString("SaveFilePicker/FileName");
    private string _defaultExtension = "txt";
    private string _folder;
    private string _defaultFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    private string _fileFilter = $"{ResourceUtil.GetString("SaveFilePicker/FileFilter/AllFiles")}|*.*";
    private uint _options;

    public SaveFilePicker SetTitle(string title)
    {
        _title = title;
        return this;
    }

    public SaveFilePicker SetFileName(string fileName)
    {
        _fileName = fileName;
        return this;
    }

    public SaveFilePicker SetDefaultExtension(string defaultExtension)
    {
        _defaultExtension = defaultExtension;
        return this;
    }

    public SaveFilePicker SetFolder(string folder)
    {
        _folder = folder;
        return this;
    }

    public SaveFilePicker SetDefaultFolder(string defaultFolder)
    {
        _defaultFolder = defaultFolder;
        return this;
    }

    public SaveFilePicker SetFileFilter(string fileFilter)
    {
        _fileFilter = fileFilter;
        return this;
    }

    public SaveFilePicker SetOptions(uint options)
    {
        _options = options;
        return this;
    }

    private static IFileSaveDialog GetIFileSaveDialog()
    {
        int hr = PInvoke.CoCreateInstance<IFileSaveDialog>(
            typeof(FileSaveDialog).GUID,
            null,
            CLSCTX.CLSCTX_INPROC_SERVER,
            out var fsd);
        if (hr < 0)
        {
            Marshal.ThrowExceptionForHR(hr);
        }

        return fsd;
    }

    private unsafe IFileSaveDialog CreateSaveFileDialog()
    {
        var fsd = GetIFileSaveDialog();

        fsd.SetTitle(_title);
        fsd.SetFileName(_fileName);

        if (!string.IsNullOrEmpty(_folder)) fsd.SetFolder(PickerUtil.ParseDirectoryShellItem(_folder));
        if (!string.IsNullOrEmpty(_defaultFolder)) fsd.SetDefaultFolder(PickerUtil.ParseDirectoryShellItem(_defaultFolder));  
        fsd.SetDefaultExtension(_defaultExtension);

        var extensions = new List<COMDLG_FILTERSPEC>();
        if (!string.IsNullOrEmpty(_fileFilter))
        {
            var tokens = _fileFilter.Split('|');
            if (0 == tokens.Length % 2)
            {
                for (var i = 1; i < tokens.Length; i += 2)
                {
                    COMDLG_FILTERSPEC extension;
                    extension.pszSpec = (char*)Marshal.StringToHGlobalUni(tokens[i]);
                    extension.pszName = (char*)Marshal.StringToHGlobalUni(tokens[i - 1]);
                    extensions.Add(extension);
                }
            }
        }

        fsd.SetFileTypes(extensions.ToArray());

        fsd.SetOptions((FILEOPENDIALOGOPTIONS)_options);

        return fsd;
    }

    public unsafe string Open()
    {
        try
        {
            var fsd = CreateSaveFileDialog();

            // Retrieve the window handle (HWND) and other setup here...
            var hWnd = WindowNative.GetWindowHandle(App.MWindow);

            fsd.Show(new HWND(hWnd));

            fsd.GetResult(out var ppsi);

            PWSTR filename;
            ppsi.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, &filename);

            return filename.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.WriteLine("A problem occurred: " + ex.Message);
            return null;
        }
    }
}

public class OpenFilePicker
{
    private string _title = ResourceUtil.GetString("OpenFilePicker/Title");
    private string _fileName = "";
    private string _defaultExtension = "";
    private string _folder;
    private string _defaultFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    private string _fileFilter = $"{ResourceUtil.GetString("OpenFilePicker/FileFilter/AllFiles")}|*.*";
    private uint _options;

    public OpenFilePicker SetTitle(string title)
    {
        _title = title;
        return this;
    }

    public OpenFilePicker SetFileName(string fileName)
    {
        _fileName = fileName;
        return this;
    }

    public OpenFilePicker SetDefaultExtension(string defaultExtension)
    {
        _defaultExtension = defaultExtension;
        return this;
    }

    public OpenFilePicker SetFolder(string folder)
    {
        _folder = folder;
        return this;
    }

    public OpenFilePicker SetDefaultFolder(string defaultFolder)
    {
        _defaultFolder = defaultFolder;
        return this;
    }

    public OpenFilePicker SetFileFilter(string fileFilter)
    {
        _fileFilter = fileFilter;
        return this;
    }

    public OpenFilePicker SetOptions(uint options)
    {
        _options = options;
        return this;
    }

    private static IFileOpenDialog GetIFileOpenDialog()
    {
        int hr = PInvoke.CoCreateInstance<IFileOpenDialog>(
            typeof(FileOpenDialog).GUID,
            null,
            CLSCTX.CLSCTX_INPROC_SERVER,
            out var fod);
        if (hr < 0)
        {
            Marshal.ThrowExceptionForHR(hr);
        }

        return fod;
    }

    private unsafe IFileOpenDialog CreateOpenFileDialog()
    {
        var fod = GetIFileOpenDialog();

        fod.SetTitle(_title);
        fod.SetFileName(_fileName);
        fod.SetDefaultExtension(_defaultExtension);

        if (!string.IsNullOrEmpty(_folder)) fod.SetFolder(PickerUtil.ParseDirectoryShellItem(_folder));
        if (!string.IsNullOrEmpty(_defaultFolder)) fod.SetDefaultFolder(PickerUtil.ParseDirectoryShellItem(_defaultFolder));

        var extensions = new List<COMDLG_FILTERSPEC>();
        if (!string.IsNullOrEmpty(_fileFilter))
        {
            var tokens = _fileFilter.Split('|');
            if (0 == tokens.Length % 2)
            {
                for (var i = 1; i < tokens.Length; i += 2)
                {
                    COMDLG_FILTERSPEC extension;
                    extension.pszSpec = (char*)Marshal.StringToHGlobalUni(tokens[i]);
                    extension.pszName = (char*)Marshal.StringToHGlobalUni(tokens[i - 1]);
                    extensions.Add(extension);
                }
            }
        }

        fod.SetOptions((FILEOPENDIALOGOPTIONS)_options);

        fod.SetFileTypes(extensions.ToArray());

        return fod;
    }

    public unsafe string Open()
    {
        try
        {
            var fod = CreateOpenFileDialog();

            // Retrieve the window handle (HWND) and other setup here...
            var hWnd = WindowNative.GetWindowHandle(App.MWindow);

            fod.Show(new HWND(hWnd));

            fod.GetResult(out var ppsi);

            PWSTR filename;
            ppsi.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, &filename);

            return filename.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine("A problem occurred: " + ex.Message);
            return null;
        }
    }
}

public static class PickerUtil
{
    internal static IShellItem ParseDirectoryShellItem(string directory)
    {
        Console.WriteLine(directory);
        int hr = PInvoke.SHCreateItemFromParsingName(
            directory,
            null,
            typeof(IShellItem).GUID,
            out var directoryShellItem);
        if (hr < 0)
        {
            Marshal.ThrowExceptionForHR(hr);
        }

        return directoryShellItem as IShellItem;
    }
}
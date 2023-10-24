using Html2Markdown;
using Microsoft.UI.Xaml;
using PolicyManager.Utils;

namespace PolicyManager.Pages.Test;

public sealed partial class TestPage
{
    public RegistryResult RegistryResult { get; set; }

    public bool IsFucking { get; set; }

    public TestPage()
    {
        InitializeComponent();
        RegistryResult = new RegistryResult("shit", "qwe");
        DataContext = RegistryResult;


        var content = "设置网站是否可以使用 W3C Web 语音 API 来识别用户的语音。Web 语音 API 的 Microsoft Edge 实施使用 Azure 认知服务，因此语音数据将离开计算机。<br><br>如果已启用或未配置此策略，则使用 Web 语音 API 的 Web 应用程序可以使用语音识别。<br><br>如果禁用此策略，则无法通过 Web 语音 API 使用语音识别。<br><br>在此处了解有关此功能的更多信息:<br>SpeechRecognition API: <a href=\"https://go.microsoft.com/fwlink/?linkid=2143388\">https://go.microsoft.com/fwlink/?linkid=2143388</a><br>认知服务: <a href=\"https://go.microsoft.com/fwlink/?linkid=2143680\">https://go.microsoft.com/fwlink/?linkid=2143680</a>\n";

        var converter = new Converter();
        var markdown = converter.Convert(content);

        // string rtfContent = "{\\rtf1\\ansi\\ansicpg1252\n{\\fonttbl\\f0\\fswiss\\fcharset0 Arial;}\n{\\colortbl;\\red255\\green0\\blue0;\\red0\\green128\\blue0;}\n\\viewkind4\\uc1\\pard\\cf1\\f0\\fs20 This is \\cf2\\ul\\i bold and underlined text\\ulnone\\i0\\cf1, and this is \\cf2\\i\\ul red and green colored text.\\ulnone\\i0\\cf1\\par\n}\n"

        // 使用Markdig解析Markdown

        // 将HTML转换为RTF格式
        // string rtfContent = HtmlToRtfConverter.ConvertHtmlToRtf(htmlContent);

        // 创建RichTextBox控件并显示RTF内容
        // LoadWebview();

        DataContext = this;
    }

    // private async void LoadWebview()
    // {
    //     const string markdownContent = "## This is a Markdown Heading\n" +
    //                                    "* Item 1\n" +
    //                                    "* Item 2\n" +
    //                                    "* Item 3";
    //     var htmlContent = Markdown.ToHtml(markdownContent);
    //     await WebView2.EnsureCoreWebView2Async();
    //     WebView2.NavigateToString(htmlContent);
    // }

    private void ToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
    {
    }
}
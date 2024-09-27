using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfRTFDocumentExtensionExample;

public static class FlowDocuments
    {
        public static FlowDocument ExampleMessage
        {
            get
            {
                return new FlowDocument()
                    .AppendText("This is an example of a message that can be displayed to the user.").AppendNewLine()
                    .AppendHighlitedText("This message can be displayed in a dialog box or a window.").AppendNewLine()
                    .AppendBoldText("  Cancel").AppendText(" = Don't show again").AppendNewLine()
                    .AppendBoldText("        OK").AppendText(" = Remind me next time").AppendNewLine();
            }
        }

        public static void LoadRTF(this FlowDocument doc, Stream stream)
        {
            var content = new TextRange(doc.ContentStart, doc.ContentEnd);

            if (content.CanLoad(DataFormats.Rtf))
            {
                content.Load(stream, DataFormats.Rtf);
            }
        }

        public static void SaveRTF(this FlowDocument doc, string file)
        {
            file.ToFileInfo().Directory.Create();
            var content = new TextRange(doc.ContentStart, doc.ContentEnd);
            if (content.CanSave(DataFormats.Rtf))
            {
                using (var stream = new FileStream(file, FileMode.OpenOrCreate))
                {
                    content.Save(stream, DataFormats.Rtf);
                }
            }
        }

        public static FlowDocument AppendNewLine(this FlowDocument doc)
        {
            if (doc == null) return null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                var range = new TextRange(doc.ContentEnd, doc.ContentEnd);
                range.Text = Environment.NewLine;
            });
            return doc;
        }

        public static FlowDocument AppendBoldText(this FlowDocument doc, string text)
        {
            if (doc == null) return null;
            return doc.AppendText(text, Brushes.Black, FontWeights.Bold);
        }

        public static FlowDocument AppendBoldText(this FlowDocument doc, string text, Brush color)
        {
            if (doc == null) return null;
            return doc.AppendText(text, color, FontWeights.Bold);
        }

        public static FlowDocument AppendText(this FlowDocument doc, string text)
        {
            if (doc == null) return null;
            return doc.AppendText(text, Brushes.Black, FontWeights.Normal);
        }

        public static FlowDocument AppendText(this FlowDocument doc, string text, FontWeight fontWeight)
        {
            if (doc == null) return null;
            return doc.AppendText(text, Brushes.Black, fontWeight);
        }

        public static FlowDocument AppendText(this FlowDocument doc, string text, Brush color)
        {
            if (doc == null) return null;
            return doc.AppendText(text, color, FontWeights.Normal);
        }

        public static FlowDocument AppendText(this FlowDocument doc, string text, Brush color, FontWeight fontWeight)
        {
            if (doc == null) return null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                var range = new TextRange(doc.ContentEnd, doc.ContentEnd);
                range.Text = text;
                range.ApplyPropertyValue(TextElement.ForegroundProperty, color);
                range.ApplyPropertyValue(TextElement.FontWeightProperty, fontWeight);
                //Unset the background so the following text wont be highlighted automatically
                range.ApplyPropertyValue(TextElement.BackgroundProperty, null);
            });
            return doc;
        }

        public static FlowDocument AppendHighlitedText(this FlowDocument doc, string text)
        {
            if (doc == null) return null;
            return doc.AppendHighlitedText(text, Brushes.Black, Brushes.Yellow, FontWeights.Normal);
        }

        public static FlowDocument AppendHighlitedText(this FlowDocument doc, string text, Brush highlight)
        {
            if (doc == null) return null;
            return doc.AppendHighlitedText(text, Brushes.Black, highlight, FontWeights.Normal);
        }

        public static FlowDocument AppendHighlitedText(this FlowDocument doc, string text, Brush color, Brush highlight, FontWeight fontWeight)
        {
            if (doc == null) return null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                var range = new TextRange(doc.ContentEnd, doc.ContentEnd);
                range.Text = text;
                range.ApplyPropertyValue(TextElement.ForegroundProperty, color);
                range.ApplyPropertyValue(TextElement.BackgroundProperty, highlight);
                range.ApplyPropertyValue(TextElement.FontWeightProperty, fontWeight);
            });
            return doc;
        }
    }

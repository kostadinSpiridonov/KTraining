using DW.RtfWriter;
using KTreining.Model;
using System;
using System.Linq;

namespace KTraining.Service
{
    public class ExportTestService : BaseService
    {
        /// <summary>
        /// Export automatic test to rtf document
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string ExportAutomaticTest(int id)
        {
            var test = this.context.AutomaticTests.Find(id);
            var doc = CreateAutomaticTestRtf(test);
            return doc.render();
        }
        
        /// <summary>
        /// Create the rtf document
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        private RtfDocument CreateAutomaticTestRtf(AutomaticTest test)
        {
            var doc = new RtfDocument(PaperSize.A4, PaperOrientation.Portrait,
            Lcid.TraditionalChinese);
            var times = doc.createFont("Times New Roman");
            RtfParagraph par;
            RtfCharFormat fmt;
            RtfImage img;
            par = doc.addParagraph();
            par.Alignment = Align.Center;
            par.DefaultCharFormat.Font = times;
            par.setText(test.Title);
            fmt = par.addCharFormat();
            fmt.FontSize = 18;
            for (int i = 0; i < test.CloseQuestions.Count; i++)
            {
                par = doc.addParagraph();
                par.Alignment = Align.Left;
                par.DefaultCharFormat.Font = times;
                par.setText((i + 1) + "." + ConvertHtmlToText(test.CloseQuestions.ElementAt(i).Content) + "  (" + test.CloseQuestions.ElementAt(i).Points + " т.)");
                fmt = par.addCharFormat();
                fmt.FontSize = 14;
                var answerIn = 'А';

                foreach (var item in test.CloseQuestions.ElementAt(i).Images)
                {
                    img = doc.addImageFromUrl(this.cloudinaryService.GetImageUrl(item.Source), item.Name
                        , ImageFileType.Jpg);
                    img.Width = 100;
                    img.Heigth = 100;
                }
                foreach (var item2 in test.CloseQuestions.ElementAt(i).Answers)
                {
                    par = doc.addParagraph();
                    par.Alignment = Align.Left;
                    par.DefaultCharFormat.Font = times;
                    par.setText(answerIn + ") " + ConvertHtmlToText(item2.Content));
                    answerIn++;
                    foreach (var item3 in item2.Images)
                    {
                        img = doc.addImageFromUrl(this.cloudinaryService.GetImageUrl(item3.Source), item3.Name
                            , ImageFileType.Jpg);
                        img.Width = 80;
                        img.Heigth = 60;
                        img.Margins[Direction.Bottom] = 5;
                        img.Margins[Direction.Left] = 5;
                        img.Margins[Direction.Right] = 5;
                        img.Margins[Direction.Top] = 5;
                    }
                }
                par = doc.addParagraph();
                par.Alignment = Align.Left;
                par.DefaultCharFormat.Font = times;
                par.setText(" ");
            }
            return doc;
        }


        /// <summary>
        /// Convert htrml to text
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertHtmlToText(string source)
        {

            string result;

            // Remove HTML Development formatting
            // Replace line breaks with space
            // because browsers inserts space
            result = source.Replace("\r", " ");
            // Replace line breaks with space
            // because browsers inserts space
            result = result.Replace("\n", " ");
            // Remove step-formatting
            result = result.Replace("\t", string.Empty);
            // Remove repeating speces becuase browsers ignore them
            result = System.Text.RegularExpressions.Regex.Replace(result,
                                                                  @"( )+", " ");

            // Remove the header (prepare first by clearing attributes)
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*head([^>])*>", "<head>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<( )*(/)( )*head( )*>)", "</head>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(<head>).*(</head>)", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // remove all scripts (prepare first by clearing attributes)
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*script([^>])*>", "<script>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<( )*(/)( )*script( )*>)", "</script>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            //result = System.Text.RegularExpressions.Regex.Replace(result, 
            //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
            //         string.Empty, 
            //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<script>).*(</script>)", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // remove all styles (prepare first by clearing attributes)
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*style([^>])*>", "<style>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<( )*(/)( )*style( )*>)", "</style>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(<style>).*(</style>)", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // insert tabs in spaces of <td> tags
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*td([^>])*>", "\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // insert line breaks in places of <BR> and <LI> tags
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*br( )*>", "\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*li( )*>", "\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // insert line paragraphs (double line breaks) in place
            // if <P>, <DIV> and <TR> tags
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*div([^>])*>", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*tr([^>])*>", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*p([^>])*>", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // Remove remaining tags like <a>, links, images,
            // comments etc - anything thats enclosed inside < >
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<[^>]*>", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // replace special characters:
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&nbsp;", " ",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&bull;", " * ",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&lsaquo;", "<",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&rsaquo;", ">",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&trade;", "(tm)",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&frasl;", "/",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<", "<",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @">", ">",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&copy;", "(c)",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&reg;", "(r)",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // Remove all others. More can be added, see
            // http://hotwired.lycos.com/webmonkey/reference/special_characters/
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&(.{2,6});", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);


            // make line breaking consistent
            result = result.Replace("\n", "\r");

            // Remove extra line breaks and tabs:
            // replace over 2 breaks with 2 and over 4 tabs with 4. 
            // Prepare first to remove any whitespaces inbetween
            // the escaped characters and remove redundant tabs inbetween linebreaks
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)( )+(\r)", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\t)( )+(\t)", "\t\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\t)( )+(\r)", "\t\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)( )+(\t)", "\r\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // Remove redundant tabs
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)(\t)+(\r)", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // Remove multible tabs followind a linebreak with just one tab
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)(\t)+", "\r\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // Initial replacement target string for linebreaks
            string breaks = "\r\r\r";
            // Initial replacement target string for tabs
            string tabs = "\t\t\t\t\t";
            for (int index = 0; index < result.Length; index++)
            {
                result = result.Replace(breaks, "\r\r");
                result = result.Replace(tabs, "\t\t\t\t");
                breaks = breaks + "\r";
                tabs = tabs + "\t";
            }

            // Thats it.
            return result;

        }

        /// <summary>
        /// Export manual test to rtf
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string ExportManualTest(int id)
        {
            var test = this.context.ManualTests.Find(id);
            var doc = CreateManualTestRtf(test);
            return doc.render();
        }

        /// <summary>
        /// Create rtf document
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        private RtfDocument CreateManualTestRtf(ManualTest test)
        {
            var doc = new RtfDocument(PaperSize.A4, PaperOrientation.Portrait,
            Lcid.TraditionalChinese);
            var times = doc.createFont("Times New Roman");
            RtfParagraph par;
            RtfCharFormat fmt;
            RtfImage img;
            par = doc.addParagraph();
            par.Alignment = Align.Center;
            par.DefaultCharFormat.Font = times;
            par.setText(test.Title);
            fmt = par.addCharFormat();
            fmt.FontSize = 18;
            for (int i = 0; i < test.CloseQuestions.Count; i++)
            {
                par = doc.addParagraph();
                par.Alignment = Align.Left;
                par.DefaultCharFormat.Font = times;
                par.setText((i + 1) + "." + ConvertHtmlToText(test.CloseQuestions.ElementAt(i).Content) + "  (" + test.CloseQuestions.ElementAt(i).Points + " т.)");
                fmt = par.addCharFormat();
                fmt.FontSize = 14;
                var answerIn = 'А';

                foreach (var item in test.CloseQuestions.ElementAt(i).Images)
                {
                    img = doc.addImageFromUrl(this.cloudinaryService.GetImageUrl(item.Source), item.Name
                        , ImageFileType.Jpg);
                    img.Width = 100;
                    img.Heigth = 100;
                }
                foreach (var item2 in test.CloseQuestions.ElementAt(i).Answers)
                {
                    par = doc.addParagraph();
                    par.Alignment = Align.Left;
                    par.DefaultCharFormat.Font = times;
                    par.setText(answerIn + ") " + ConvertHtmlToText(item2.Content));
                    answerIn++;
                    foreach (var item3 in item2.Images)
                    {
                        img = doc.addImageFromUrl(this.cloudinaryService.GetImageUrl(item3.Source), item3.Name
                            , ImageFileType.Jpg);
                        img.Width = 80;
                        img.Heigth = 60;
                        img.Margins[Direction.Bottom] = 5;
                        img.Margins[Direction.Left] = 5;
                        img.Margins[Direction.Right] = 5;
                        img.Margins[Direction.Top] = 5;
                    }
                }
                par = doc.addParagraph();
                par.Alignment = Align.Left;
                par.DefaultCharFormat.Font = times;
                par.setText(" ");
            }
            for (int i = 0; i < test.OpenQuestions.Count; i++)
            {
                par = doc.addParagraph();
                par.Alignment = Align.Left;
                par.DefaultCharFormat.Font = times;
                par.setText((i + 1 + test.CloseQuestions.Count) + "." + ConvertHtmlToText(test.OpenQuestions.ElementAt(i).Content) + "  (" + test.OpenQuestions.ElementAt(i).Points + " т.)");
                fmt = par.addCharFormat();
                fmt.FontSize = 14;

                foreach (var item in test.OpenQuestions.ElementAt(i).Images)
                {
                    img = doc.addImageFromUrl(this.cloudinaryService.GetImageUrl(item.Source), item.Name
                        , ImageFileType.Jpg);
                    img.Width = 100;
                    img.Heigth = 100;
                }
            }
            return doc;
        }

    }
}

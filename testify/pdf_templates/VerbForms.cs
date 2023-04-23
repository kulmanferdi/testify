using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using testify.source;

namespace testify.pdf_templates
{
    public class VerbForms : Pdf
    {
        public void GenerateDoc(List<Dictionary> wordlist)
        {
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    int rowNum = 1;
                    int rows = 15;
                    string[] word = { "apple", "banana" };

                    page.Size(PageSizes.A5.Landscape());
                    page.Margin(7, Unit.Millimetre);
                    page.MarginTop(5, Unit.Millimetre);

                    page.Header().AlignLeft().AlignMiddle().DebugArea()
                        .Height(1, Unit.Centimetre)
                        .Text("Név:").FontSize(13);

                    page.Content().MaxWidth(400)
                        .PaddingVertical(10)
                        .PaddingHorizontal(10)
                        .DebugArea()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            // by using custom 'Element' method, we can reuse visual configuration
                            foreach (var unused in Enumerable.Range(0, rows))
                            {
                                table.Cell().Row((uint)rowNum).Column(1).Element(Block)
                                    .Text(unused % 2 == 0 ? word[0] : word[1]);
                                table.Cell().Row((uint)rowNum).Column(2).Element(Block).Text("");
                                rowNum++;
                            }

                            // for simplicity, you can also use extension method described in the "Extending DSL" section
                            static IContainer Block(IContainer container)
                            {
                                return container
                                    .Border(2)
                                    .ShowOnce()
                                    .MaxWidth(200)
                                    .MinHeight(22)
                                    .AlignCenter()
                                    .AlignMiddle();
                            }
                        });

                });
            }).ShowInPreviewer();
        }
    }
}
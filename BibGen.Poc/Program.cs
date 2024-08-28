using BibGen.Services;
using BibGen.Svc.Model;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddSingleton<IBibImageGenerator, BibImageGenerator>()
    .AddSingleton<IBibDataLoader, BibDataLoader>()
    .AddSingleton<IBibPipeline, BibPipeline>()
    .AddSingleton<IPdfGenerator, PdfGenerator>()
    .BuildServiceProvider();

var bibSvc = serviceProvider
    .CreateAsyncScope().ServiceProvider
    .GetRequiredService<IBibPipeline>();



bibSvc.GenerateBibs(new BibGenContext
{
    ExcelFilePath = @"Resources\6. Noćna 10ka Zagreb i polumaraton - HACL.xlsx",
    BackgroundFilePath = @"Resources\background.jpeg",
    OutputFilePath = @"Resources\Output",
    MarginStripes = new List<float> { 0.2f, 0.2f, 0.2f, 0.2f },
    LineDescriptors = new List<List<LineDescriptor>>
    {
        new List<LineDescriptor> {
            new LineDescriptor
            {
                FontName = @"Resources\Fonts\Barlow\BarlowCondensed-Bold.otf",
                FontSize = 300f,
                BaseDistance = 0.25f
            },
        },
        new List<LineDescriptor> {
            new LineDescriptor
            {
                FontName = @"Resources\Fonts\Barlow\BarlowCondensed-Bold.otf",
                FontSize = 210f,
                BaseDistance = 0.44f
            },
            new LineDescriptor
            {
                FontName = @"Resources\Fonts\Barlow\Barlow-Extra Bold.ttf",
                FontSize = 80f,
                BaseDistance = 0.22f
            }
        }
    }
});

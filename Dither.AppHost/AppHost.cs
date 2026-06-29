var builder = DistributedApplication.CreateBuilder(args);

var ditherApi =
    builder.AddProject<Projects.Service_Dither_Presentation_WebApi>("ditherApi")
        .WithUrl("Scalar", "/scalar");

var quoteApi = builder.AddProject<Projects.Service_Quote_WebApi>("quoteApi")
    .WithUrl("Scalar", "/scalar");

var ditherMvc =
    builder.AddProject<Projects.Service_WebSite_Presentation_Mvc>("ditherMvc")
        .WithReference(ditherApi)
        .WithReference(quoteApi);

builder.Build().Run();
@echo off
dotnet restore src\Easy.Web
dotnet pack --output NuGet --configuration Release src\Easy.Web

dotnet restore src\Easy.Web.Serialization.Jil
dotnet pack --output NuGet --configuration Release src\Easy.Web.Serialization.Jil

dotnet restore src\Easy.Web.Serialization.JSONNet
dotnet pack --output NuGet --configuration Release src\Easy.Web.Serialization.JSONNet

dotnet restore src\Easy.Web.Serialization.NetJSON
dotnet pack --output NuGet --configuration Release src\Easy.Web.Serialization.NetJSON

dotnet restore src\Easy.Web.Serialization.ProtobufNet
dotnet pack --output NuGet --configuration Release src\Easy.Web.Serialization.ProtobufNet

dotnet restore src\Easy.Web.Serialization.XML
dotnet pack --output NuGet --configuration Release src\Easy.Web.Serialization.XML
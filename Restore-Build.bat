@echo off
dotnet restore src\Easy.Web
dotnet build src\Easy.Web

dotnet restore src\Easy.Web.Serialization.Jil
dotnet build src\Easy.Web.Serialization.Jil

dotnet restore src\Easy.Web.Serialization.JSONNet
dotnet build src\Easy.Web.Serialization.JSONNet

dotnet restore src\Easy.Web.Serialization.NetJSON
dotnet build src\Easy.Web.Serialization.NetJSON

dotnet restore src\Easy.Web.Serialization.ProtobufNet
dotnet build src\Easy.Web.Serialization.ProtobufNet

dotnet restore src\Easy.Web.Serialization.XML
dotnet build src\Easy.Web.Serialization.XML
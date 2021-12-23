FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS build
WORKDIR /src

# Install Nuget Packages ahead of other files for layer caching
COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
COPY src/Drudoca.Blog.sln Drudoca.Blog.sln
RUN dotnet restore "Drudoca.Blog.sln"

# Build the project
COPY src .
WORKDIR "/src/Drudoca.Blog.Web"
RUN dotnet build "Drudoca.Blog.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Drudoca.Blog.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Drudoca.Blog.Web.dll"]

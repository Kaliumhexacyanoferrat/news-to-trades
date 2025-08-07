# News 2 Trades

This repository contains a simple console app that will

- periodically check various configured sources for headlines
- ask a locally installed LLM for strong buy signals for news associated stock symbols

## Running the App

As this is a personal toy project, there are no comfort features at all, so you will probably need to adjust the source code according to your needs.

The following commands expect the .NET SDK and Docker to be installed.

```bash
docker-compoe up -d

# load the model of your choice (once)
docker exec -it ollama ollama pull llama3

cd news-to-trades

dotnet run
```

The app will then yield stock symbols once a minute.

## Customization

You probably want to:

- adjust the sources in `NewsAggregator.cs`
- adjust the prompt in `prompt.txt`
- select a different model in `NewsAnalyzer.cs`

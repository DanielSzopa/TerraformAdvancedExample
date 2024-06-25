FROM mcr.microsoft.com/azure-storage/azurite:latest

ENTRYPOINT [ "azurite", "-l", "/data", "--blobHost", "0.0.0.0", "--queueHost", "0.0.0.0", "--tableHost", "0.0.0.0" ]
CMD [ "--blobPort", "7777", "--queuePort", "7778", "--tablePort", "7779" ]
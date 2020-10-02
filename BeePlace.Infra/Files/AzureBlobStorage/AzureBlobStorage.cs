using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace BeePlace.Infra.AzureBlobStorage
{
    /// <summary>
    /// Classe responsável por gerenciar conexões e armazenamento de arquivos no Azure Blob Storage
    /// </summary>
    public class AzureBlobStorage
    {
        /// <summary>
        /// Método responsável por armazenar arquivos no Azure Blob Storage
        /// </summary>
        /// <param name="filePath">Caminho do arquivo que será armazenado ex: "@"\Arquivos\"</param>
        /// <param name="fileName">Nome o arquivo que será armazenado com extensão ex: "documento.jpg"</param>
        /// <param name="connection">String de conexão com a conta Azure Blob Storage</param>
        /// <param name="containerName">Nome do Container onde o arquivo será armazenado</param>
        /// <returns>Objeto Azure Blob preenchido</returns>
        public static CloudBlob Upload(string filePath, string fileName, string connection, string containerName)
        {
            CloudBlockBlob blob;
            try
            {
                // Recupera a conta com base na conexão
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connection);

                // Cria o cliente
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Recupera o container.
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                // Recupera o blob a partir do container e do nome do arquivo.
                blob = container.GetBlockBlobReference(fileName);

                // Atribui o content type do arquivo conforme a extensão.
                blob.Properties.ContentType = AzureBlobStorage.GetContentType(fileName);

                // Armazena ou sobrepoe o arquivo
                using (var fileStream = File.OpenRead(filePath + fileName))
                {
                    blob.UploadFromStream((fileStream));
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return blob;
        }

        /// <summary>
        /// Método responsável por armazenar arquivos no Azure Blob Storage
        /// </summary>
        /// <param name="content">Conteudo do arquivo que será armazenado"</param>
        /// <param name="fileName">Nome o arquivo que será armazenado com extensão ex: "documento.jpg"</param>
        /// <param name="connection">String de conexão com a conta Azure Blob Storage</param>
        /// <param name="containerName">Nome do Container onde o arquivo será armazenado</param>
        /// <returns>Objeto Azure Blob preenchido</returns>
        public static CloudBlob UploadFromString(string content, string fileName, string connection, string containerName)
        {
            CloudBlockBlob blob;
            try
            {
                // Recupera a conta com base na conexão
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connection);

                // Cria o cliente
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Recupera o container.
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                // Recupera o blob a partir do container e do nome do arquivo.
                blob = container.GetBlockBlobReference(fileName);

                // Atribui o content type do arquivo conforme a extensão.
                blob.Properties.ContentType = AzureBlobStorage.GetContentType(fileName);

                blob.UploadText(content);
            }
            catch (Exception e)
            {
                throw e;
            }

            return blob;
        }


        /// <summary>
        /// Método responsável por armazenar arquivos no Azure Blob Storage
        /// </summary>
        /// <param name="stream">Objeto Stream contendo o arquivo a ser armazenado</param>
        /// <param name="fileName">Nome o arquivo que será armazenado com extensão ex: "documento.jpg"</param>
        /// <param name="connection">String de conexão com a conta Azure Blob Storage</param>
        /// <param name="containerName">Nome do Container onde o arquivo será armazenado</param>
        /// <returns>Objeto Azure Blob preenchido</returns>
        public static CloudBlob Upload(Stream stream, string fileName, string connection, string containerName)
        {
            CloudBlockBlob blob;
            try
            {
                // Recupera a conta com base na conexão
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connection);

                // Cria o cliente
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Recupera o container.
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                // Recupera o blob a partir do container e do nome do arquivo.
                blob = container.GetBlockBlobReference(fileName);

                // Atribui o content type do arquivo conforme a extensão.
                blob.Properties.ContentType = AzureBlobStorage.GetContentType(fileName);

                // Armazena ou sobrepoe o arquivo
                blob.UploadFromStream(stream);
            }
            catch (Exception e)
            {
                throw e;
            }

            return blob;
        }

        /// <summary>
        /// Método responsável por recuperar arquivos no Azure Blob Storage
        /// </summary>
        /// <param name="fileName">Nome o arquivo que será armazenado com extensão ex: "documento.jpg"</param>
        /// <param name="connection">String de conexão com a conta Azure Blob Storage</param>
        /// <param name="containerName">Nome do Container onde o arquivo será armazenado</param>
        /// <returns>Objeto Azure Blob preenchido</returns>
        public static CloudBlob Download(string fileName, string connection, string containerName)
        {
            CloudBlockBlob blob;
            try
            {
                // Recupera a conta com base na conexão
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connection);

                // Cria o cliente
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Recupera o container.
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                // Recupera o blob a partir do container e do nome do arquivo.
                blob = container.GetBlockBlobReference(fileName);

                MemoryStream memStream = new MemoryStream();

                blob.DownloadToStream(memStream);
            }
            catch (Exception e)
            {
                throw e;
            }

            return blob;
        }


        /// <summary>
        /// Método responsável por remover arquivos no Azure Blob Storage
        /// </summary>
        /// <param name="fileName">Nome o arquivo que será armazenado com extensão ex: "documento.jpg"</param>
        /// <param name="connection">String de conexão com a conta Azure Blob Storage</param>
        /// <param name="containerName">Nome do Container onde o arquivo será armazenado</param>
        /// <returns>Objeto Azure Blob preenchido</returns>
        public static CloudBlob Delete(string fileName, string connection, string containerName)
        {
            CloudBlockBlob blob;

            try
            {
                // Recupera a conta com base na conexão
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connection);

                // Cria o cliente
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Recupera o container.
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                // Recupera o blob a partir do container e do nome do arquivo.
                blob = container.GetBlockBlobReference(fileName);

                blob.Delete();
            }
            catch (Exception e)
            {
                throw e;
            }

            return blob;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString">String de conexão com a conta Azure Blob Storage</param>
        /// <param name="containerName">Nome do Container onde o sub diretório estará armazenado/param>
        /// <param name="directoryName">Nome do sub diretório onde os arquivos estarão aramazenados</param>
        /// <returns></returns>
        public static List<string> ListBobsUriInDirectoty(string connectionString, string containerName, string directoryName)
        {
            IEnumerable<IListBlobItem> listBlobItem = null;
            List<string> blobsUri = new List<string>();

            try
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve a reference to a container.
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                // Retrieve a reference to a directoty.
                CloudBlobDirectory directory = container.GetDirectoryReference(directoryName);

                // List blobs in the directoty.
                listBlobItem = directory.ListBlobs(true).ToList();

                foreach( var blobItem in listBlobItem)
                {
                    blobsUri.Add(blobItem.Uri.AbsoluteUri);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return blobsUri;
        }

        private static string GetContentType(string fileName)
        {
            string contentType;
            var extension = Path.GetExtension(fileName)?.ToUpper();

            switch (extension)
            {
                case ".PDF":
                    contentType = "application/pdf";
                    break;
                case ".TXT":
                    contentType = "text/plain";
                    break;
                case ".BMP":
                    contentType = "image/bmp";
                    break;
                case ".GIF":
                    contentType = "image/gif";
                    break;
                case ".PNG":
                    contentType = "image/png";
                    break;
                case ".JPG":
                    contentType = "image/jpeg";
                    break;
                case ".JPEG":
                    contentType = "image/jpeg";
                    break;
                case ".XLS":
                    contentType = "application/vnd.ms-excel";
                    break;
                case ".XLSX":
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".CSV":
                    contentType = "text/csv";
                    break;
                case ".HTML":
                    contentType = "text/html";
                    break;
                case ".XML":
                    contentType = "text/xml";
                    break;
                case ".JSON":
                    contentType = "application/json";
                    break;
                case ".ZIP":
                    contentType = "application/zip";
                    break;
                default:
                    contentType = "application/octet-stream";
                    break;
            }

            return contentType;
        }
    }
}

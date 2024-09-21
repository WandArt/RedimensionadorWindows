using System;
using System.Drawing;
using System.Threading;

namespace Atividades
{
    internal class Program
    {
        static void Main(string[] args
        )
        {
            Console.WriteLine("Iniciando redimensionador!");

            Thread thread = new System.Threading.Thread(Redimensionar);
            thread.Start();


            Console.Read();

        }



        static void Redimensionar()
        {
            #region Diretorios

            string diretorio_entrada = "Arquivos_entrada";
            string diretorio_redimensionados = "Arquivos_redimensionados";
            string diretorio_finalizados = "Arquivos_Finalizados";

            if (!Directory.Exists(diretorio_entrada))
            {
                Directory.CreateDirectory(diretorio_entrada);
            }

            if (!Directory.Exists(diretorio_redimensionados))
            {
                Directory.CreateDirectory(diretorio_redimensionados);
            }
            if (!Directory.Exists(diretorio_finalizados))
            {
                Directory.CreateDirectory(diretorio_finalizados);
            }
            #endregion

            while (true)
            {
                //Projeto olhar para a pasta de entrada
                //Se tiver arquivo, ele irá redimensionar
                var arquivosEntrada = Directory.EnumerateFiles(diretorio_entrada);


                //Ler o tamanho que irá redimensionar
                int novaAltura = 200;

                foreach (var arquivo in arquivosEntrada)
                {
                    FileStream fileStream = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    FileInfo fileInfo = new FileInfo(arquivo);

                    string caminho = Environment.CurrentDirectory + @"\" + diretorio_redimensionados + @"\"
                    + fileInfo.Name + DateTime.Now.Millisecond.ToString();

                    //Redimenisona
                    Redimensionador(Image.FromStream(fileStream), novaAltura, diretorio_redimensionados);

                    //fecha o arquivo
                    fileStream.Close();
                    string caminhoFinalizado = Environment.CurrentDirectory + @"\" + diretorio_finalizados;
                    //Copia os reimenisonados para a pasta de redimensionados
                    fileInfo.MoveTo(caminhoFinalizado);


                    //Move arquivos de entrada para a pasta de finalizados
                }
                Thread.Sleep(new TimeSpan(0, 0, 5));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagem"> imagem a ser redimensionada</param>
        /// <param name="altura">Altura que desehjamos redimensionar</param>
        /// <param name="caminho">caminho aonde iremos gravar o arquivo redimensionar</param>
        /// <returns></returns>
        static void Redimensionador(Image imagem, int altura, string caminho)
        {
            double ratio = (double)altura / imagem.Height;
            int novaLargura = (int)(imagem.Width * ratio);
            int novaAltura = (int)(imagem.Height * ratio);

            Bitmap novaImage = new Bitmap(novaLargura, novaAltura);

            using (Graphics g = Graphics.FromImage(novaImage))
            {
                g.DrawImage(imagem, 0, 0, novaLargura, novaAltura);
            }


            novaImage.Save(caminho);
            imagem.Dispose();
        }
    }

}
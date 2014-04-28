using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace netfis.Clases
{
   
    class IONetfis{

        //Lee datos de un archivo de texto
        public static String leer(String ruta)
        {
            using (FileStream fs = File.OpenRead(ruta))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF7))// poniendo Enconding.UTF7 te lee también ñ y tildes
                {
                    while (sr.Peek() != -1)
                    {
                        return sr.ReadToEnd();//poniendo ReadtoEnd te lee todo el archivo y ya no una sola linea
                    }
                    return null;
                }
            }
        }

        //Escribe datos en un archivo de texto
        public static bool escribir(String texto, String ruta)
        {
            try
            {
                using (FileStream Fs = new FileStream(ruta, FileMode.Append, FileAccess.Write))//FileMode.Append para que escriba al final del texto
                {
                    using (StreamWriter sw = new StreamWriter(Fs))
                    {
                        sw.WriteLine(texto);
                        sw.Close();
                    }
                    Fs.Close();
                }
                return true;
            }
            catch(Exception excepcion)
            {
                return false;
            }
            finally
            {

            }
        }

        //Lee archivos en memoria byte por byte
        public void Memoria(string ruta)
        {
            //
            try
            {
                using (FileStream Fs = new FileStream(ruta, FileMode.OpenOrCreate, FileAccess.Read))//crea un stream para abrir el archivo en modo lectura
                {
                    byte[] b = new byte[Fs.Length];//crea un array de bytes
                    Fs.Read(b, (int)0, (int)Fs.Length);// lee byte por byte el stream y se pasa al array
                    using (MemoryStream Ms = new MemoryStream(b))//crea un memorystream con el array para leer en memoria
                    {

                        System.Windows.MessageBox.Show(Encoding.UTF7.GetString(Ms.ToArray()));//muestra byte por byte el contenido del memorystream
                        Ms.Close();

                    }
                    Fs.Close();
                }
            }
            catch (Exception excepccion)
            {

                System.Windows.MessageBox.Show("Error, consulte a su Administrador de Sistema");
            }
            //
        }

        //Comprimir archivo en formato gz,zip,etc
        public void comprimir(string ruta,string nombre_comprimido_extension)
        {
            try
            {
            using (FileStream fs = new FileStream(ruta, FileMode.Open, FileAccess.Read))//crea un stream para abrir el archivo en modo lectura
            {
                using (FileStream fsc = new FileStream(nombre_comprimido_extension, FileMode.OpenOrCreate, FileAccess.Write))//crea un stream para crear el archivo y escribir sobre el
                {
                    using (GZipStream gz = new GZipStream(fsc, CompressionMode.Compress))
                    {

                        int i = fs.ReadByte();
                        while (i != -1)
                        {
                            gz.WriteByte((byte)i);
                            i = fs.ReadByte();
                        }
                        gz.Close();
                    }
                    fsc.Close();
                }
                fs.Close();
            }
            }
            catch (Exception excepccion)
            {

                System.Windows.MessageBox.Show("Error, consulte a su Administrador de Sistema");
            }
        }
        //Descomprimir archivo gz,zip,etc
        public void descomprimir(string ruta,string archivo_descomprimido)
        {

            try{

            using (FileStream fs = new FileStream(ruta, FileMode.Open, FileAccess.Read))
            {
                using (FileStream fsd = new FileStream(archivo_descomprimido, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (GZipStream gs = new GZipStream(fs, CompressionMode.Decompress))
                    {
                        int i = gs.ReadByte();
                        while (i != -1)
                        {
                            fsd.WriteByte((byte)i);
                            i = gs.ReadByte();

                        }
                        gs.Close();
                    }
                    fsd.Close();
                }
                fs.Close();
            }
            }
            catch (Exception excepccion)
            {

                System.Windows.MessageBox.Show("Error, consulte a su Administrador de Sistema");
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

using System.IO.Compression;
using System.Security.Cryptography;
using System.IO.MemoryMappedFiles;
using System.IO.IsolatedStorage;

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
        public void Leer_en_memoria(string ruta)
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

<<<<<<< HEAD
=======
        public byte[] bkey; 
        public byte[] bIV;
        public void encriptar(string archivo_origen, string archivo_encriptado)
        {
            try
            {
                TripleDESCryptoServiceProvider Algoritmo = new TripleDESCryptoServiceProvider();
                Algoritmo.GenerateKey();
                Algoritmo.GenerateIV();
                bkey = Algoritmo.Key;
                bIV = Algoritmo.IV;
                ICryptoTransform Transformacion = Algoritmo.CreateEncryptor();
                using (FileStream FsDestino = new FileStream(archivo_encriptado, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (CryptoStream Cs = new CryptoStream(FsDestino, Transformacion, CryptoStreamMode.Write))
                    {
                        string Contenido = File.ReadAllText(archivo_origen);
                        using (StreamWriter Sw = new StreamWriter(Cs))
                        {
                            Sw.Write(Contenido);
                            Sw.Close();
                        }
                    }
                    FsDestino.Close();
                }

            }
            catch (Exception exc)
            {
                System.Windows.MessageBox.Show("Error, consulte a su Administrador de Sistema");
            }
        }
        // desencriptas el archivo ,ojo solo habiendo encriptado el archivo(igual se consistenciará en el evento)
        public void Desencriptar(string archivo_origen, string archivo_desencriptado)
        {

            try
            {

                TripleDESCryptoServiceProvider Algoritmo = new TripleDESCryptoServiceProvider();
                Algoritmo.Key = bkey;
                Algoritmo.IV = bIV;
                ICryptoTransform Transformacion = Algoritmo.CreateDecryptor();
                using (FileStream FsOrigen = new FileStream(archivo_origen, FileMode.Open, FileAccess.Read))
                {
                    using (CryptoStream Cs = new CryptoStream(FsOrigen, Transformacion, CryptoStreamMode.Read))
                    {
                        using (StreamReader Sr = new StreamReader(Cs))
                        {
                            string Cadena = Sr.ReadToEnd();
                            File.WriteAllText(archivo_desencriptado, Cadena);
                        }
                    }
                    FsOrigen.Close();
                }

            }
            catch (Exception excp)
            {
                
                System.Windows.MessageBox.Show("Error, consulte a su Administrador de Sistema");

            }
        }

        //Escribes dentro de un archivo especificando de donde a donde en el texto
        public void escribir_en_memoria(string ruta,string texto,int ini_texto,int tamaño_texto)
        {
            try
            {
                using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(ruta, FileMode.OpenOrCreate, "Demo", 100))
                {
                    using (MemoryMappedViewStream vista = mmf.CreateViewStream(ini_texto,tamaño_texto))
                    {
                        byte[] bTexto = System.Text.Encoding.UTF7.GetBytes(texto);

                        vista.Write(bTexto, 0, bTexto.Length);
                    }
                }
            }
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show("Error, consulte a su Administrador de Sistema");
            }
        }
        //Crear un directorio en un almacenamiento aislado y dentro de el crea un archivo y ademas se escribe texto dentro del archivo
        public void directorio_en_almacenamiento_aislado(string carpeta, string archivo,string texto)
        {
            try
            {
                IsolatedStorageFile Almacen = IsolatedStorageFile.GetUserStoreForApplication();

                Almacen.CreateDirectory(carpeta);

                using (IsolatedStorageFileStream Fs = new IsolatedStorageFileStream(archivo, FileMode.OpenOrCreate, Almacen))
                {
                    using (StreamWriter Sw = new StreamWriter(Fs))
                    {
                        Sw.WriteLine(texto);
                        Sw.Close();
                    }
                    /*
                     Otra forma de escribir la cadena (byte por byte)
                    string C = texto_que_quieres_escribir;
                    byte[] b = System.Text.Encoding.UTF8.GetBytes(C);
                    Fs.Write(b, 0, b.Length);*/
                    Fs.Close();
                }


            }
            catch (Exception Exp)
            {
                System.Windows.MessageBox.Show("Error, consulte a su Administrador de Sistema");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data

namespace netfis.Clases
{
    //la primera app
    class IONetfis{

        //Lee datos de un archivo de texto
        public static String leer(String ruta)
        {
            using (FileStream fs = File.OpenRead(ruta))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (sr.Peek() != -1)
                    {
                        return sr.ReadLine();
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
                using (FileStream Fs = new FileStream(ruta, FileMode.OpenOrCreate, FileAccess.Write))
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
    }
}

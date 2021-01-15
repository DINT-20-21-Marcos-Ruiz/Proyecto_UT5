
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Newtonsoft.Json;


namespace Proyecto_UT5
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Pelicula pelicula;
        ObservableCollection<Pelicula> lista;
        ObservableCollection<Pelicula> listaJugar;
        int contador;
        int posicion = 0;
        int puntuacionDificultad;
        Random random;
        public MainWindow()
        {
            
            InitializeComponent();
            lista = new ObservableCollection<Pelicula>();
            //PELÍCULAS DE PRUEBA
            /*lista.Add(new Pelicula("Rifkin's Festival", "Rifkin's Festival", "Resources/prueba.jpg",Dificultad.Normal,Genero.CienciaFiccion));
            lista.Add(new Pelicula("PIRATAS DEL CARIBE", "PIRATAS DEL CARIBE", "Resources/prueba.jpg", Dificultad.Dificil, Genero.Terror));
            lista.Add(new Pelicula("Rebeca", "Rebeca", "Resources/prueba.jpg", Dificultad.Facil, Genero.Comedia));
            lista.Add(new Pelicula("Madame Curie", "Madame Curie", "Resources/prueba.jpg", Dificultad.Dificil, Genero.Accion));
            lista.Add(new Pelicula("Antebellum", "Antebellum", "Resources/prueba.jpg", Dificultad.Normal, Genero.Comedia));
            lista.Add(new Pelicula("La caza", "La caza", "Resources/prueba.jpg", Dificultad.Dificil, Genero.CienciaFiccion));
            lista.Add(new Pelicula("Richard Jewell", "Richard Jewell", "Resources/prueba.jpg", Dificultad.Facil, Genero.Drama));
            lista.Add(new Pelicula("La candidata perfecta", "La candidata perfecta", "Resources/prueba.jpg", Dificultad.Normal, Genero.Drama));*/
            peliculas_ListBox.DataContext = lista;

            //NUEVA PELÍCULA
            pelicula = new Pelicula();
            newPelicula_StackPanel.DataContext = pelicula;

            //JUGAR PELÍCULA
            if (lista.Count > 0)
            {
                peliculas_Random();
                jugarPelicula_Grid.DataContext = listaJugar[posicion];
                totalPag_TextBlock.Text = Convert.ToString(listaJugar.Count);
                pagActual_TextBlock.Text = "1";
            }
            
        }


        //BOTONES LATERALES GESTIÓN ----------------------------------------------------------------------------->
        private void cargarJSON_Button_Click(object sender, RoutedEventArgs e)
        {         
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "*.json|*.json";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (openFileDialog.ShowDialog() == true)
                {
                    using (StreamReader jsonStream = File.OpenText(openFileDialog.FileName))
                    {
                        var json = jsonStream.ReadToEnd();
                        List<Pelicula> peliculas = JsonConvert.DeserializeObject<List<Pelicula>>(json);

                        foreach (Pelicula p in peliculas)
                        {
                            lista.Add(p);
                        }
                    }
                }
                MessageBox.Show("Película importada con éxito", "Películas", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void guardarJSON_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "*.json|*.json";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (saveFileDialog.ShowDialog() == true)
                {
                    string peliculasJson = JsonConvert.SerializeObject(lista);
                    File.WriteAllText(saveFileDialog.FileName, peliculasJson);
                }
                MessageBox.Show("Película exportada con éxito", "Películas", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception)
            {
                throw;
            }
            
        }

        private void eliminarPel_Button_Click(object sender, RoutedEventArgs e)
        {
            lista.Remove((Pelicula)peliculas_ListBox.SelectedItem);
            MessageBox.Show("Película eliminada con éxito", "Películas", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void añadirPel_Button_Click(object sender, RoutedEventArgs e)
        {
            if(tituloPelGestionar_TextBox.Text == "" || pistaPelGestionar_TextBox.Text == "" || imagenPelGestionar_TextBox.Text == "")
            {
                MessageBox.Show("Complete todos los campos", "Películas", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                lista.Add(pelicula);
                contador = Convert.ToInt32(totalPag_TextBlock.Text) + 1;
                totalPag_TextBlock.Text = Convert.ToString(contador);
                pelicula = new Pelicula();
                newPelicula_StackPanel.DataContext = pelicula;
                MessageBox.Show("Película insertada con éxito", "Películas", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
        }

        private void examinar_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "*.jpg|*.jpg|*.png|*.png";
                if (openFileDialog.ShowDialog() == true)
                {
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    imagenPelGestionar_TextBox.Text = openFileDialog.FileName;

                    //Añado esta línea ya que el binding de "imagenPelGestionar_TextBox" no me lo insertaba si lo hacía mediante "Examinar"
                    pelicula.Imagen = openFileDialog.FileName;
                }
                    
                
            }
            catch (Exception)
            {
                throw;
            }
        }


        //FLECHAS AVANZAR-RETROCEDER ----------------------------------------------------------------------------->
        private void Retroceder_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (posicion > 0)
            {
                posicion -= 1;
                jugarPelicula_Grid.DataContext = listaJugar[posicion];
                pagActual_TextBlock.Text = Convert.ToString(posicion + 1);
                tituloPel_TextBox.Text = "";
                verPista_CheckBox.IsChecked = false;
            }
        }

        private void Avanzar_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AvanzarArrow();
        }
        private void AvanzarArrow()
        {
            if (posicion + 1 < listaJugar.Count)
            {
                posicion += 1;
                jugarPelicula_Grid.DataContext = listaJugar[posicion];
                pagActual_TextBlock.Text = Convert.ToString(posicion + 1);
                tituloPel_TextBox.Text = "";
                verPista_CheckBox.IsChecked = false;
            }
        }


        //VALIDAR PELÍCULA ----------------------------------------------------------------------------->
        private void validar_Button_Click(object sender, RoutedEventArgs e)
        {
            if (lista.Count >= 5)
            {
                if (tituloPel_TextBox.Text == listaJugar[posicion].Titulo.ToString())
                {
                    switch (pelicula.Dificultad)
                    {
                        case Dificultad.Facil:
                            puntuacionDificultad = 5;
                            break;
                        case Dificultad.Normal:
                            puntuacionDificultad = 10;
                            break;
                        case Dificultad.Dificil:
                            puntuacionDificultad = 15;
                            break;

                    }
                    if ((Boolean)verPista_CheckBox.IsChecked) puntuacionDificultad /= 2;
                    punTotal_TextBlock.Text = (Convert.ToInt32(punTotal_TextBlock.Text) + puntuacionDificultad).ToString();
                    tituloPel_TextBox.Text = "";
                    verPista_CheckBox.IsChecked = false;
                    AvanzarArrow();
                    MessageBox.Show("¡Acertaste la película!", "Películas", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("¡Fallaste!", "Películas", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Tienen que haber mínimo 5 películas en la lista", "Películas", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }

        private ObservableCollection<Pelicula> peliculas_Random()
        {
            listaJugar= new ObservableCollection<Pelicula>();
            random = new Random();
            List<int> repetidos = new List<int>();
            int posicionRandom;

            for (int i = 0; i < 5; i++)
            {

                posicionRandom = random.Next(0, lista.Count);
                if(repetidos.Count > 0)
                {
                    while (repetidos.Contains(posicionRandom))
                    {
                        posicionRandom = random.Next(0, lista.Count);
                    }
                }           
                listaJugar.Add(lista[posicionRandom]);
                repetidos.Add(posicionRandom);


            }
       
            return listaJugar;
        }

        //NUEVA PARTIDA
        private void nuevaPartida_Button_Click(object sender, RoutedEventArgs e)
        {
            tituloPel_TextBox.Text = "";
            verPista_CheckBox.IsChecked = false;
            punTotal_TextBlock.Text = Convert.ToString(0);

            peliculas_Random();
            jugarPelicula_Grid.DataContext = listaJugar[posicion];
            totalPag_TextBlock.Text = Convert.ToString(listaJugar.Count);
            pagActual_TextBlock.Text = "1";

        }
    }
}

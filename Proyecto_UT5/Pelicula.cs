using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_UT5
{
    enum Dificultad { Facil, Normal, Dificil }
    enum Genero { Comedia, Drama, Accion, Terror, CienciaFiccion }
    class Pelicula : INotifyPropertyChanged
    {
        private string _titulo;
        private string _pista;
        private string _imagen;
        private Dificultad _dificultad;
        private Genero _genero;

        public string Titulo
        {
            get { return this._titulo; }
            set
            {
                if (this._titulo != value)
                {
                    this._titulo = value;
                    this.NotifyPropertyChanged("Titulo");
                }
            }
        }
        public string Pista
        {
            get { return this._pista; }
            set
            {
                if (this._pista != value)
                {
                    this._pista = value;
                    this.NotifyPropertyChanged("Pista");
                }
            }
        }
        public string Imagen
        {
            get { return this._imagen; }
            set
            {
                if (this._imagen != value)
                {
                    this._imagen = value;
                    this.NotifyPropertyChanged("Imagen");
                }
            }
        }
        public Dificultad Dificultad
        {
            get { return this._dificultad; }
            set
            {
                if (this._dificultad != value)
                    this._dificultad = value;
                this.NotifyPropertyChanged("Dificultad");
            }
        }
        public Genero Genero
        {
            get { return this._genero; }
            set
            {
                if (this._genero != value)
                    this._genero = value;
                this.NotifyPropertyChanged("Genero");
            }
        }

        public Pelicula()
        {

        }

        public Pelicula(string titulo, string pista, string imagen, Dificultad dificultad, Genero genero)
        {
            _titulo = titulo;
            _pista = pista;
            _imagen = imagen;
            _dificultad = dificultad;
            _genero = genero;
        }



        //INOTIFY
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

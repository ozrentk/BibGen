using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BibGen.App.Commands
{
    public static class BibGenCommands
    {
        public static readonly RoutedCommand AddStripe = new RoutedCommand();
        public static readonly RoutedCommand UpdateStripe = new RoutedCommand();
        public static readonly RoutedCommand RemoveStripe = new RoutedCommand();
        public static readonly RoutedCommand SelectStripe = new RoutedCommand();
        public static readonly RoutedCommand DeselectStripe = new RoutedCommand();
    }
}

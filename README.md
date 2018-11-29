MainPage.xaml

<Label  x:Name="WelcomeTextLabel"
    Text="{Binding WelcomeText}" 
    HorizontalOptions="Center"
    VerticalOptions="Center" />
<Button Text="Change" Clicked="ButtonClicked"  />

Notate come la property Text della label è in bound con WelcomeText.  WelcomeText sarà il name di una property nel ViewModel corrispondente. 
Convenzionalmente la classe ViewModel ha lo stesso nome della classe View class ma finisce con ViewModel.
Quindi, la View è MainPage e il ViewModel è MainPageViewModel.

Quindi grazie al binding sulla property del controllo diciamo a Xamarin.Forms: “Quando la property cambia aggiorna il controllo.”

Creiamo quindi la classe del ViewModel MainPageViewModel.cs ed aggiungiamo la property che vogliamo mettere in bind. 

private string welcomeText = "Hello world"; 
public string WelcomeText
 {
    get => welcomeText;
    set => SetValue( ref welcomeText, value );
 }

Affinchè il databinding funzioni abbiamo bisogno 4 ingredienti:

la keyword Binding nello XAML (fatto!)
un BindingContext
la property da mettere in binding
l'implementazione di INotifyPropertyChanged

Il BindingContext è un modo per dire a Xamarin.Forms dove trovare le properties che possono essere oggetto di binding.
Nel nostro caso il BindingContext è MainPageViewModel, che dobbiamo indicare in MainPage.xaml.cs

public MainPage()
 {
    InitializeComponent();
    var vm = new MainPageViewModel();
    BindingContext = vm;
 }

La property da mettere in binding è nel ViewModel, ora implementiamo INotifyPropertyChanged. Siccome questa interfaccia va implementata in tutte le classi ViewModel ci conviene creare un  base viewmodel.
Il "mio" base view model in particolare crea il metodo SetValue che rende il processo di binding più semplice. 

public abstract class BaseViewModel : INotifyPropertyChanged
 {
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged( 
           [CallerMemberName] string propertyName = null )
    {
       PropertyChanged?.Invoke( 
               this, new PropertyChangedEventArgs( propertyName ) );
    }

    protected void SetValue<T>( ref T   backingField,
          T       value,
          [CallerMemberName] string propertyName = null )
    {
       if (EqualityComparer<T>.Default.Equals( 
                    backingField, value )) return;
       backingField = value;
       OnPropertyChanged( propertyName );
    }
}

La prima riga dichiara l'evento PropertyChanged cje è richiesto e definito dall'interfaccia.
Subito dopo c'è un metodo event handler che si chiama OnPropertyChanged che è abbastanza standard: controlla se qualcuno è registrato sull'evento e se è così scatena l'evento con il nome della property che è stata aggiornata.

Il metodo SetValue è sostanzialmente un helper per semplificare l'aggiornamento della property.

Facciamo il percorso inverso del  binding.
Se qualcuno (vedi il button handler) setta il valore di WelcomeText nel ViewModel, la variabile sottostante (welcomeText) si aggiornerà e NotifyPropertyChanged sarà lanciato. Visto che il view è registrata su di esso (dal binding context), il controllo in bounding con questa property (la label) sarà aggiornato.

Manca solo chi scatena tutto il meccanismo

        private void ButtonClicked(object sender, EventArgs e)
        {
            WelcomeTextLabel.Text = "Ciao a tutti!";
        }


Ma c'è di più!

Abbiamo usato un event handler per il button collegato al metodo nel code behind. 
Questo è poco elegante, rende il codice disordinato e non permette di scrivere unit tests ad esempio.
Quindi dobbiamo spostare il concetto di event handling nel View Model. Per far questo si usano i Command

<Button Text="Change" Command="{Binding ButtonClickedCommand}"/>

Un command è come una property nel senso che si può fare bind con un command nello XMAL.
Nel view model dichiariamo il command come property.

public ICommand ButtonClickedCommand { get; set; }

poi, nel costruttore del View Model istanzionamo il command fornendogli il nome nel metodo che fa da handler.

public MainPageViewModel()
{
   ButtonClickedCommand = new Command(OnButtonClicked);
}

Quindi quando il button è clicked il command richiama il metodo OnButtonClicked del View Model.

private void OnButtonClicked()
{
   WelcomeText = "Ciao da Bound Command!!";
}

Notate che nel OnButtonClicked settiamo il text nella  property WelcomeText e basta. ù
Non settiamo la label direttamente.
Settando la property WelcomeText viene richiamata la property NotifyChanged e la label tramite bind aggiorna il proprio valore.


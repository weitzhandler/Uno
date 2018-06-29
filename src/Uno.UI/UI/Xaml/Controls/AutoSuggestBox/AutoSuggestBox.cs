
using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

namespace Windows.UI.Xaml.Controls
{
	public  partial class AutoSuggestBox : ItemsControl
	{
		private TextBox _textBox;
		private Popup _popup;
		private Border _suggestionContainer;
		private ListView _suggestionsList;
		private Button _queryButton;

		public AutoSuggestBox() : base()
		{
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_textBox = GetTemplateChild("TextBox") as TextBox;
			_popup = GetTemplateChild("SuggestionsPopup") as Popup;
			_suggestionContainer = GetTemplateChild("SuggestionsContainer") as Border;
			_suggestionsList = GetTemplateChild("SuggestionsList") as ListView;
			_queryButton = GetTemplateChild("QueryButton") as Button;

			_textBox.SetBinding(
				TextBox.TextProperty,
				new Binding() {
					Path = "Text",
					RelativeSource = RelativeSource.TemplatedParent
				}
			);

			Loaded += (s, e) => RegisterEvents();
			Unloaded += (s, e) => UnregisterEvents();

			if (IsLoaded)
			{
				RegisterEvents();
			}
		}

		void RegisterEvents()
		{
			_textBox.KeyDown += OnTextBoxKeyDown;
			_queryButton.Click += OnQueryButtonClick;
			_suggestionsList.ItemClick += OnSuggestionListItemClick;
		}

		private void OnSuggestionListItemClick(object sender, ItemClickEventArgs e)
		{
			SuggestionChosen?.Invoke(this, new AutoSuggestBoxSuggestionChosenEventArgs(e.ClickedItem));
		}

		private void OnQueryButtonClick(object sender, RoutedEventArgs e) => SubmitSearch();

		private void SubmitSearch()
		{
			QuerySubmitted.Invoke(this, new AutoSuggestBoxQuerySubmittedEventArgs(null, Text));
		}

		private void OnTextBoxKeyDown(object sender, KeyRoutedEventArgs e)
		{
			if(e.Key == System.VirtualKey.Enter)
			{
				SubmitSearch();
			}
		}

		private static void OnTextChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			if(dependencyObject is AutoSuggestBox tb)
			{
				tb.TextChanged.Invoke(tb, new AutoSuggestBoxTextChangedEventArgs() { Reason = AutoSuggestionBoxTextChangeReason.UserInput });
			}
		}

		void UnregisterEvents()
		{
			_textBox.KeyDown -= OnTextBoxKeyDown;
			_queryButton.Click -= OnQueryButtonClick;
			_suggestionsList.ItemClick -= OnSuggestionListItemClick;
		}
	}
}

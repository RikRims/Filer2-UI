// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Filer2_UI.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace Filer2_UI.Views.Pages;
public partial class DashboardPage : INavigableView<DashboardViewModel>
{
	public DashboardViewModel ViewModel { get; }

	public DashboardPage(DashboardViewModel viewModel)
	{
		ViewModel = viewModel;
		DataContext = this;
		
		InitializeComponent();
		ViewModel.WorkPreraration();
	}
}

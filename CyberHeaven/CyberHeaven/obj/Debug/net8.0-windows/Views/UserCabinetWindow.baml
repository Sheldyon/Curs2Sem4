   M S B A M L   `   `   `  ����F  BCyberHeaven, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:CyberHeaven.ViewsCyberHeaven.Views  5   	   ?$clr-namespace:CyberHeaven.ConvertersCyberHeaven.Converters  5   	   )    #CyberHeaven.Views.UserCabinetWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:CyberHeaven.Views     5   	   ;
converters$clr-namespace:CyberHeaven.Converters     5   	     � Icon$7  1D:\ООП\CyberHeaven\CyberHeaven\images\icon.ico��5   
    � Title$! Личный кабинет��5
   	    � WindowState$ 	Maximized=�5   	    � WindowStyle$
 None=�5   	    � AllowsTransparency$
 True��5   	    � 	Resources 5       �� MergedDictionaries �� Source6   0CyberHeaven.Converters.NullToVisibilityConverter   NullToVisibilityConverter0   *CyberHeaven.Converters.NullToTextConverter  NullToTextConverter1   +CyberHeaven.Converters.ThemeToIconConverter  ThemeToIconConverter4   .CyberHeaven.Converters.InverseBooleanConverter  InverseBooleanConverter9   3CyberHeaven.Converters.StatusToBorderBrushConverter   StatusToBorderBrushConverter5   /CyberHeaven.Converters.RatingToBooleanConverter  RatingToBooleanConverter>   8CyberHeaven.Converters.StatusToCancelVisibilityConverter % !StatusToCancelVisibilityConverter@   :CyberHeaven.Converters.InverseBooleanToVisibilityConverter ' #InverseBooleanToVisibilityConverter7	   1CyberHeaven.Converters.CountToVisibilityConverter  CountToVisibilityConverter  	 BooleanToVisibilityConverter7
   1CyberHeaven.Converters.BlockedPlaceColorConverter 
 BlockedPlaceColorConverter9   3CyberHeaven.Converters.BlockedPlaceTooltipConverter   BlockedPlaceTooltipConverter�� 5   
    5      �� 5      $ /Resources/Strings.xaml?�6%   %\  &	        &	      &	 &     &	 9     &	 L     &	 _     &	 r     &	 �     &	 �     &		 �     &	
 �     &	 �       5      63     5      6-     5      6.     5      61     5      66     5      62     5      6;     5      6=   	  5       64   �� 5!      6+   
  5$      64     5'      66   5)      .��5,      � .+�5.   
   ��   MainBackground#���  6   $��UniformToFill=�6:   ]� 51   
   .+�53      ]� $	��Top=�6   $��10q�6-   .+�54      �� $
��Left=�6     ProfileButtonStyle#��[ 56      $��94��6E   $	��114��6Q   ��55      �� *OpenMainCommand+.��57      �� $��70��6=   $��70��6H   ��6   �� *UserImagePath+6H   58      �� 5:       �� Orientation$ 
Horizontal=�6   $��Right=�66   $��Center=�6R   .+�5;      �� $��10q�6M     BaseButtonStyle#��[ 5<      ��5;      �� *Localization[BookingButton]+��5<   F   �� *OpenBronCommand+�� 5=      $��10q�6J   #��[ 5>      ��5=      �� *Localization[MainButton]+��5>   F   �� *OpenMainCommand+�� 5?      #��[ 6O   $��10q�6x   ��6   �� *Localization[ReviewButton]+��6�   �� *OpenReviewCommand+5@      5A      � 5D      $��20q�6   	 � ColumnDefinitions	 5E      �� 5F      $	��450 �6'   �� 5G      $��* �6'   .+�5K      �� $��0��6     UserPanelBackgroundStyle#��[ 6)   ._�5L      �� $
��Auto=�6#   .��5M      �� $��20q�6%   .+�5O      �� 
 �� CornerRadius$	
 100��6%   $	��150��68   $	��150��6D   $��Center=�5P   %   $	��Top=�6B     LightBackgroundBrush#���  5Q   %   ._�5R   "   �� $��UniformToFill=�6I   ��6(   �� *UserImagePath+ J� Clip 5S   &   @� 5T   *    @� Center$ 75,75&�6:    @� RadiusX$ 75Q�6I    @� RadiusY$ 75Q�6V   5V   #   5W      �� 5Y        EditProfileButtonStyle#��[ 5[   %   $��0,15,0,0q�5\   %   ��5Y   %   �� *" Localization[ChangeAvatarButton]+��5Z   %   �� *ChangeAvatarCommand+5\   %   �� 5^      .+�5`   "   �� $��0,20,0,0q�6Y     PrimaryDarkerBrush#���  6k   ��6,   �� *Localization[UsernameLabel]+6k   �� 5a   "   $	��0,5q�6u    �� 
IsReadOnly$
 True��6�     AuthTextBoxStyle#��[ 6�   ��6*   �� *CurrentUser.Username+ �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5b   "   $��0,10,0,0q�6V   #���  6h   ��6,   �� *Localization[EmailLabel]+6h   �� 5c   "   $	��0,5q�6r   $
 True��6�   #��[ 6�   ��6*   �� *CurrentUser.Email+$ PropertyChanged=�6�   �� 5d   "   $��0,10,0,0q�6V   #���  6i   ��6,   �� *Localization[PhoneLabel]+6i   �� 5e   "   $
 True��6s   $	��0,5q�6�   #��[ 6�   ��6*   �� *CurrentUser.Phone+$ PropertyChanged=�6�   5f      �� 5j      $ Vertical=�6)   $��0,30,0,0q�6@   .+�5k   "   ��   BookButtonStyle#��[ 5l   N   ��5k   )   �� *!Localization[EditProfileButton]+��5l   '   �� *EditProfileCommand+6N   �� 5m   "   #��[ 5n   K   $��0,15,0,0q�5o   )   ��5m   )   �� *Localization[LogoutButton]+��5n   )   �� *LogoutCommand+5o   )   �� 5p   "   $��10q�6Y   $��24(�6f   #��[ 5q      ��5p   )   �� *Localization[ExitAppButton]+��5q   *   �� *ExitCommand+5r      �� 5t      $��1��6%   $��0,20,0,0q�65   ���#202020$
 10��5v   .     PrimaryBrush#���  5w      $��2q�6F   y�5u      �� *
IsEditMode+ �� 	Converter# [	 ._�5x   "   �� $��20q�6-   .+�5y   &   �� $��24(�5z   $   $
��Bold"�62   #���  5{   $   $��0,0,0,20q�5|   $   $��Center=�5}   $   ��5y   0   �� *!Localization[EditProfileButton]+5}   $   �� 5   &   .+�5�   *   �� $��0,0,0,5q�6a   
���������64   �� *Localization[UsernameLabel]+6r   �� 5�   *   $��0,0,0,2q�6u   ��62   �� *TempUsername+$ PropertyChanged=�6u   �� 5�   *   
���  ��$��0,0,0,10q�6d   ��64   �� *UsernameError+6d   �� 5�   *   $��0,0,0,5q�6^   
���������64   �� *Localization[EmailLabel]+6o   �� 5�   *   $��0,0,0,2q�6r   ��62   �� *	TempEmail+$ PropertyChanged=�6r   �� 5�   *   
���  ��$��0,0,0,10q�6a   ��64   �� *
EmailError+6a   �� 5�   *   $��0,0,0,5q�6^   
���������64   �� *Localization[PhoneLabel]+6o   �� 5�   *   $��0,0,0,2q�6r   ��62   �� *	TempPhone+$ PropertyChanged=�6r   �� 5�   *   
���  ��$��0,0,0,10q�6a   ��64   �� *
PhoneError+6a   5�   '   �� 5�   &   $ 
Horizontal=�61   $��Center=�6J   $
��0,20q�6g   .+�5�   *   �� $	��100��6^   $��0,0,10,0q�6j   #���  5�   %   ��5�   1   �� *Localization[SaveButton]+��5�   %   �� *SaveProfileCommand+��6N   ��  �� ElementName$ CurrentPasswordBox��5�   %   �� 5�   *   $	��100��6`   ��61   �� *Localization[CancelButton]+��5�   %   �� *CancelEditCommand+5�   '   5�   #   5�      5�      5�      5�      �� 5�      $��1��6   $��20,0,0,0q�6)   
�������$��1q�6N   ._�5�      � .+�5�      �� $
��Auto=�6'   .��5�      �� .+�5�   "   �� $��16(�6\   $��20q�6j     	TextBrush#���  6v   $
��Bold"�6�   ��6,   �� * Localization[MyBookingsHeader]+6�   �� 5�   "   $��0,0,0,20q�6P   #���  6b   ��6/   �� *
Bookings+��5�   &   �� 5�   *   .R�5�   .   �� 
�������$��1q�6M   $��
20,0,20,10q�6a   $
 5��5�   %    �� Padding$ 10q�66   ��5�   %   �� *Status+._�5�   2   �� .+�5�   6   �� $
��Bold"�6\   #���  6n   $��14(�6�   ��6@   �� *
Place.Name+6�   �� 5�   6   #���  6c   ��6@   �� *Place.Description+6c   �� 5�   6   #���  6�   ��6@   �� *SelectedDate+ �� StringFormat$ Дата: {0:dd.MM.yyyy}��6�   �� 5�   6   �� 5�   *   $�Время: ��6.   #���  6=    �� 5�   *   #���  6e   �6.   �� *	StartTime+$ 
{0:hh\:mm}��6e    �� 5�   *   $	� - ��6.    �� 5�   *   #���  6c   �6.   �� *	EndTime+$ 
{0:hh\:mm}��6c   5�   7   �� 5�   6   #���  6|   ��6@   �� *
TotalPrice+$ Сумма: {0} руб.��6|   �� 5�   6   #���  6X   $��0,5,0,0q�6�   $
��Bold"�6�   $
��Left=�5�      $
��Auto��5�       �� TextAlignment$ Center=�5�      ��5�   @   �� *Status+��6�   �� *Status+# [ 5�      �� 5�   6   $/��)Отменить бронирование��6=   $	��380��6]   $��16(�6j   $��0,10,0,0q�5�   -     CancelButtonStyle#��[ 5�   -   ��5�   -   �� *" DataContext.CancelBookingCommand+ �� RelativeSource ��  �� AncestorType ItemsControl��5�   -   �� *Id+5�   -   5�   3   5�   /   5�   +   5�   #   5�      5�      5�      5�      5�      5�      �� 5�   
   $ 
Horizontal=�6   $
��Left=�6.   $��Bottom=�6I   .+�5�      ��   FlagButtonStyle#��[ 6@   $��	20,0,0,20q�6i   ��6   �� *SwitchToRussianCommand+.��5�      ��   RussianFlag#��[ 6   $��24��6>   $��16��6I   5�      �� 5�      #��[ 6@   $��	20,0,0,20q�6i   ��6   �� *SwitchToEnglishCommand+.��5�      ��   EnglishFlag#��[ 6   $��24��6>   $��16��6I   5�      �� 5�      $��	20,0,0,20q�5�      ��5�      �� *ToggleThemeCommand+ � ToolTip 5�      �� *" Localization[ToggleThemeTooltip]+��5�      �� 5�       �� 
TargetType" ��6     ThemeToggleButtonStyle �� BasedOn# [ 60   .�5�      ��  �� Property �@�Cursor  DefaultCursor �� Value# �  63    �� Triggers 5�      P� 5�       P� Property �@J�IsMouseOver P� Value$
 True��6=   . �5�   "   ��  �@�Cursor  PointerCursor# �  6;   5�      5�      .��5�      �� $��24��5�      $��24��6   ��5�      �� *IsDarkTheme+# [ 5�      5�      5�      5�      5�      
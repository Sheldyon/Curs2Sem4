   M S B A M L   `   `   `  ����F  BCyberHeaven, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:CyberHeaven.ViewsCyberHeaven.Views  5   	   ;"clr-namespace:CyberHeaven.ControlsCyberHeaven.Controls  5      ?$clr-namespace:CyberHeaven.ViewModelsCyberHeaven.ViewModels  5	   	   ?$clr-namespace:CyberHeaven.ConvertersCyberHeaven.Converters  5
   
   "    CyberHeaven.Views.BronWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:CyberHeaven.Views     5   	   7controls"clr-namespace:CyberHeaven.Controls     5      ;
viewmodels$clr-namespace:CyberHeaven.ViewModels     5	   	   ;
converters$clr-namespace:CyberHeaven.Converters     5
   
     � Icon$7  1D:\ООП\CyberHeaven\CyberHeaven\images\icon.ico��5   
    � Title$ Бронирование��5   	    � WindowState$ 	Maximized=�5   	    � WindowStyle$
 None=�5   	    � AllowsTransparency$
 True��5   	    � 	Resources 5   
    �� MergedDictionaries �� Source6   0CyberHeaven.Converters.NullToVisibilityConverter   NullToVisibilityConverter0   *CyberHeaven.Converters.NullToTextConverter  NullToTextConverter1   +CyberHeaven.Converters.ThemeToIconConverter  ThemeToIconConverter4   .CyberHeaven.Converters.InverseBooleanConverter  InverseBooleanConverter3   -CyberHeaven.Converters.NullToBooleanConverter  NullToBooleanConverter6   0CyberHeaven.Converters.BoolToVisibilityConverter  BoolToVisibilityConverter7   1CyberHeaven.Converters.BlockedPlaceColorConverter  BlockedPlaceColorConverter9   3CyberHeaven.Converters.BlockedPlaceTooltipConverter   BlockedPlaceTooltipConverter�� 5       5      �� 5      $ /Resources/Strings.xaml?�6)   %�   &	        &	      &	 &     &	 9     &	 L     &	 _     &	 r     &	 �       5      67     5      61     5      62     5      65     5      60     5      63     5#      68     5&      6:   5(      ��5,      �� 5-   
     MainBackground �� ImageSource# �  6   	 j� Stretch$	 UniformToFill=�6D   .��50      � .+�52   
   ]� $��-2,22,2,-22q�6   .+�54      ]� $	��Top=�6   $��10q�6-   .+�55      �� $
��Left=�6    	 ProfileButtonStyle#��[	 57      $��94��6:   $	��114��6F   ��56      �� *OpenAuthCommand+.��58      �� $��70��5;      $��70��6   ��58      �� *UserImagePath+ 
 UserIcon
 �� FallbackValue#
 [
  �� TargetNullValue# [
 5;      5=      �� 5?       �� Orientation$ 
Horizontal=�6   $��Right=�66   $��Center=�6R   .+�5B      �� $��10q�6J     BaseButtonStyle#��[ 6W   ��6   �� *Localization[MainButton]+��6�   �� *OpenMainCommand+5D      �� 5E      $��10q�6K   #��[ 6Y   ��6   �� *Localization[SalesButton]+��6�   �� *OpenSalesCommand+5G      �� 5H      #��[ 6M   $��10q�6�   ��6   �� *Localization[ReviewButton]+��6v   �� *OpenReviewCommand+6�   5J      5K      �� 5L      $��1��6   $
��Auto=�6(   $��Disabled=�6K   .��5M      �� .+�5N      � $	��Top=�6   $	��350��60   .+�5P      ��   MainHeaderImage#���  6    $	��Top=�5Q      $��Uniform=�6   $	��350��5R      $��0q�6   � 5S      .+�5T      �� $��Center=�6)   $��Center=�6D   .+�5U   "   �� -   5         ��Name$ 	CyberText��5U   ,     	CyberText#��[ 5V        LogoTextStyle#��[ 5W      $��Center=�5X      ${�False��63   �� 5Z   "   -   5        $ 
HeavenText��5Z   ,     
HeavenText#��[ 5[      #��[ 5\      $��Center=�5]      $��0,10,0,0q�5^      5_      5`      5a      �� 5d      $��Bottom=�6   $��10��65   $��0q�6A    �� Padding$ 0,0,0,0q�6L     DividerGradient#���  6_   5e      �� 5f      .+�5g      � $��20q�6    � ColumnDefinitions 5h      �� 5i   "   $
��Auto �63   �� 5j   "   $��* �63   .+�5m      �� $��0��5n        MapImage#���  5o      $��Uniform=�5p      $	��600��5q      $��Center=�5r      $��
200,0,0,20q�5s      �� 5v      $��1��6+   $��Center=�6c   ��6;   �� *PlaceCategories+��5w   "   �� 5x   &   .R�5y   *   �� $
��0,20q�65   .+�5z   .   ��   InputBackgroundBrush#���  5{      $��18(�5|      $
��Bold"�5}      $��Center=�5~      ��5z   8   �� *Name+5~      � 5�   .    � Orientation$ 
Horizontal=�68   $��Center=�5�      $	��0,5q�5�      .+�5�   2   �� ��6?   �� *Places+��5�   6   �� 5�   :   .R�5�   >   � $ 
Horizontal=�6H   5�   ;   ��5�   6   �� 5�   :   .R�5�   >   �� $��3q�5�   %   $��60��5�   %   $��60��5�   %   $��16(�5�   %   $
��Bold"�5�   %   ��5�   E   �� *Name+��5�   %   �� * DataContext.SelectPlaceCommand+ �� RelativeSource ��  �� AncestorType
 Window��5�   %   �� ��5�   B   �� 5�   F    �� 
TargetType" ��6L     PlaceButtonStyle �� BasedOn# [ 6`   .�5�   J   ��  �� Property � �� �� Value
 � � �6g   �� 5�   J    � ��
 �����6g   �� 5�   J    � ��  PrimaryBrush# �  6h   �� 5�   J    � �� 5�   N   �� 5�   R    �� 
TargetType" ��6b   .R�5�   V   � .+�5�   Z   ��  ��Name$ ButtonBorder��6a   #��z��5�   E   #��z��5�   E   #��z��5�   E    �� CornerRadius$ 0��5�   E   ._�5�   ^   �� $��Center=�6o   $��Center=�6�   5�   [   5�   W    �� Triggers 5�   V   P� 5�   Z    P� Property �@J�IsMouseOver P� Value$
 True��6y   . �5�   ^   ��  �� 
TargetName$ ButtonBorder��6e    �@��CornerRadius$ 30��6�   5�   [   P� 5�   Z    �@J�IsMouseOver$ False��6y   . �5�   ^   �� $ ButtonBorder��6e    �@��CornerRadius$ 0��6�   5�   [   5�   S   5�   K    �� Triggers 5�   J   P� 5�   N    �@J�IsMouseOver$
 True��6m   . �5�   R   ��  �@�Cursor  PointerCursor# �  6k   5�   O   �� 5�   N    �� Binding 6Z   �� *
IsBooked+  �� Value  True6w   .a�5�   R   ��  � ��
 �����6o   �� 5�   R    � ��$ ЗАНЯТО��6l   �� 5�   R    � {�$ False��6n   �� 5�   R    �@�Cursor# �  6k   5�   O   �� 5�   N    6Z   �� *	IsBlocked+  True6x   .a�5�   R   ��  � ��
 �  ��6o   �� 5�   R    � ��
 �����6o   �� 5�   R    � ��$  ЗАБЛОКИРОВАНО��6l   �� 5�   R    � {�$ False��6n   �� 5�   R    �@�Cursor# �  6k   5�   O   5�   G   5�   ?   5�   ;   5�   3   5�   /   5�   +   5�   '   5�      5�      �� 5�      $ 
Horizontal=�6%   $��Center=�6>   $
��0,10q�5�      y�5�      �� *AdminPanelVisibility+.+�5�      ��   AdminButtonStyle#��[ 5�      $��5q�5�      ��5�   %   �� * Localization[BlockPlaceButton]+��5�      �� * TogglePlaceAvailabilityCommand+5�      �� 5�      #��[ 5�      $��5q�5�      ��5�   %   �� *" Localization[UnblockPlaceButton]+��5�      �� *UnblockPlaceCommand+5�      �� 5�      #��[ 5�      ��5�   %   �� *Localization[AddPlaceButton]+��5�      �� *AddPlaceCommand+��5�      �� *SelectedPlace+5�      �� 5�      #��[ 5�      ��5�   %   �� *!Localization[RemovePlaceButton]+��5�      �� *RemovePlaceCommand+��5�      �� *SelectedPlace+5�      �� 5�      #��[ 5�      ��5�   %   �� *!Localization[SaveChangesButton]+��5�      �� *SavePlacesCommand+��5�      �� *SelectedPlace+5�      5�      �� 5�      $��1��6!   ���#202020$ 10��5�      $ 20q�5�      $	��800��5�      $��Center=�5�      $��	0,30,0,20q�5�      ._�5�      �� .+�5�   "   �� $ 
Horizontal=�6-   $��Center=�6F   $��0,0,0,15q�6c   .+�5�   &   �� $��5q�5�   	   $
��10,5q�6     SortButtonStyle#��[ 5�   	   $	��150��5�   	   $��40��6   $��14(�6!   ! � Cursor#! �  60   ��5�   -   �� *PriceSortButtonContent+��5�   	   �� *SortByPriceDescCommand+5�   0   �� 5�   &   $��5q�5�   	   $
��10,5q�6   #��[ 5�   	   $	��150��5�   	   $��40��6   $��14(�6!   #! �  60   ��5�   -   �� *$"Localization[SortByCategoryButton]+��5�   	   �� *SortByCategoryAscCommand+5�   0   5�   #   �� 5�   "   
�������$��18(�5�      ��5�   ,   �� *" Localization[PlaceCategoryLabel]+5�      �� 5   "   " �� DisplayMemberPath$
" Name��5     $��0,5,0,15q�5     $��16(�5       ComboBoxItemStyle#��[ 6"   ��5   +   �� *PlaceCategories+# �� SelectedItem# 5     �� *SelectedCategory+5  #   �� 5
  "   
�������$��18(�5  (   ��5
  ,   �� *" Localization[SpecificPlaceLabel]+5  (   �� 5  "   $
" Name��5     $��0,5,0,15q�5     $��16(�5     ��5  +   �� *AvailablePlaces+# 5     �� *SelectedPlace+��5  &   �� 5  *   " ��60   .�5  .   ��  � {� 6J   �� *true+$ �� 	Converter#$ [ �� 5  .    � �� 6K   �� *	IsBlocked+#$ [ �� 5  .    �@�ToolTip 6H   �� *	IsBlocked+#$ [ �� 5  .    �@�Cursor# �  6G   5  +   5  #   �� 5  "   
�������$��18(�5     $��0,10,0,5q�5     ��5  ,   �� * Localization[BookingDateLabel]+5     (	  "System.Windows.Controls.DatePicker	  5   "   #! �  5!  -   $��0,0,0,15q�5#     $��16(�5$     $��Stretch=�5%     % 	  SelectedDate% 5   -   �� *SelectedDate+& 	  DisplayDateStart& 5"     �� *DisplayDateStart+' �� Mode$' OneWay=� 5&  &   :
  4System.Windows.Controls.Primitives.DatePickerTextBox&   System.Windows.Controls.Calendar  CalendarButtonStyle��%Q  '
        '  �     &	 �     �� 5'  *   " 
 60   .�5(  .   ��  � ��
 �   �6K   �� 5)  .    � ��
 �����6K   �� 5*  .    �@�Cursor# �  6G   5+  +   �� 5,  *   "  60   .�5-  .   ��  �@�Cursor# �  6G   5.  +   �� 5/  *   " ��60   .�50  .   ��  �@�Cursor# �  6G   51  +   52  '   54  #   � 56  "    57  &   �� 58  *   $��* �6;   �� 59  *   $��* �6;   .+�5<  &   �� $��0��61   .+�5=  *   �� 
�������$��18(�5?  0   ��5=  4   �� *Localization[StartTimeLabel]+5?  0   ,   &CyberHeaven.Controls.TimeRangeSelector  5@  *   (   MinDuration$( 01:00:00h�5C     $��0,5,0,15q�5D     )   AvailableTimes) 5@  E   �� *AvailableTimes+*   	StartTime* 5A     �� *	StartTime+$' TwoWay=�+   EndTime+ 5B     �� *	EndTime+$' TwoWay=�5D     �� 5E  *   
���  ��$
��Wrap=�5H     $��0 5 0 0q�5I     ��5E  4   �� *	TimeError+y�5G     �� *HasTimeError+#$ [ 5I     5J  '   5M  #   �� 5P  "   
�������$��18(�5R  (   ��5P  ,   �� *Localization[PromoCodeLabel]+5R  (   G� 5U  "   -   5        , G�Name$, PromoCodePasswordBox��5U  .   $��0,5,0,15q�5V     $��16(�5W     - G� PasswordChar$- *��5X     5Y     �� 5]  "   #���  5^  (   $��20(�5_  (   $
��Bold"�5`  (   $��Center=�5a  (   $
��0,10q�5b  (   ��5]  ,   �� *
TotalPrice+. �� StringFormat$.  {0:N2} BYN.��5b  (   �� 5e  "     BookButtonStyle#��[ 5g  %   $
��0,20q�5h  %   ��5e  )   �� *$"Localization[ConfirmBookingButton]+��5f  %   �� *BookCommand+5h  %   5i     5k     5m     5n     5o     5q     �� 5r  
   $ 
Horizontal=�6   $
��Left=�6.   $��Bottom=�6I   .+�5s     ��   FlagButtonStyle#��[ 6@   $��	20,0,0,20q�6i   ��6   �� *SwitchToRussianCommand+.��5t     ��   RussianFlag#��[ 6   $��24��6>   $��16��6I   5u     �� 5v     #��[ 6@   $��	20,0,0,20q�6i   ��6   �� *SwitchToEnglishCommand+.��5w     ��   EnglishFlag#��[ 6   $��24��6>   $��16��6I   5x     �� 5y     $��	20,0,0,20q�5z     ��5y     �� *ToggleThemeCommand+/ � ToolTip/ 5{     �� *" Localization[ToggleThemeTooltip]+��5|     �� 5}     " ��6     ThemeToggleButtonStyle# [ 60   .�5~     ��  �@�Cursor  DefaultCursor# �  63    5     P� 5�      �@J�IsMouseOver$
 True��6=   . �5�  "   ��  �@�Cursor# �  6;   5�     5�     .��5�     �� $��24��5�     $��24��6   ��5�     �� *IsDarkTheme+#$ [ 5�     5�     5�     5�     5�     
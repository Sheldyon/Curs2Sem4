   M S B A M L   `   `   `  ����F  BCyberHeaven, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:CyberHeaven.ViewsCyberHeaven.Views  5   	   ?$clr-namespace:CyberHeaven.ConvertersCyberHeaven.Converters  5   	   #    CyberHeaven.Views.AdminWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:CyberHeaven.Views     5   	   ;
converters$clr-namespace:CyberHeaven.Converters     5   	     � Icon$7  1D:\ООП\CyberHeaven\CyberHeaven\images\icon.ico��5   
    � Title$/ )Панель администратора��5
   	    � WindowState$ 	Maximized=�5   	    � WindowStyle$
 None=�5   	    � AllowsTransparency$
 True��5   	    � 	Resources 5       �� MergedDictionaries �� Source6   0CyberHeaven.Converters.NullToVisibilityConverter   NullToVisibilityConverter1   +CyberHeaven.Converters.ThemeToIconConverter  ThemeToIconConverter9   3CyberHeaven.Converters.BooleanToVisibilityConverter   BooleanToVisibilityConverter4   .CyberHeaven.Converters.InverseBooleanConverter  InverseBooleanConverter9   3CyberHeaven.Converters.StatusToBorderBrushConverter   StatusToBorderBrushConverter3   -CyberHeaven.Converters.BooleanToTextConverter  BooleanToTextConverter@   :CyberHeaven.Converters.StatusToCompleteVisibilityConverter ' #StatusToCompleteVisibilityConverter@   :CyberHeaven.Converters.InverseBooleanToVisibilityConverter ' #InverseBooleanToVisibilityConverter>	   8CyberHeaven.Converters.StatusToCancelVisibilityConverter % !StatusToCancelVisibilityConverter�� 5   
    5      �� 5      $ /Resources/Strings.xaml?�6%   %  &	        &	      &	 &     &	 9     &	 L     &	 _     &	 r     &	 �     &	 �       5      63     5      6.     5      66     5      61     5      66     5      60     5      6=     5      6=   	  5      6;   5      .��5       � .+�5"   
   ��  	 MainBackground#��� 	 6   $��UniformToFill=�6:   ]� 5%   
   .+�5'      ]� $	��Top=�6   $��10q�6-   .+�5(      �� $
��Left=�6    
 ProfileButtonStyle#��[
 5*      $��94��6E   $	��114��6Q   ��5)      �� *OpenMainCommand+.��5+      �� $=��7D:\ООП\CyberHeaven\CyberHeaven\images\admin_icon.png��6   $��70��6Z   $��70��6e   5,      �� 5.      $��24(�5/      $
��Bold"�6*     PrimaryBrush#���  50      $��Center=�51      ��5.      �� *Localization[AdminPanelTitle]+51      �� 53       �� Orientation$ 
Horizontal=�6   $��Right=�66   $��Center=�6R   .+�54      �� $��24(�6J   $��10q�6X     BaseButtonStyle#��[ 55      ��54      �� *Localization[MainButton]+��55   F   �� *OpenMainCommand+�� 56      $��10q�6M   $��24(�6Z   #��[ 57      ��56      �� *Localization[LogoutButton]+��57   F   �� *LogoutCommand+�� 58      $��10q�6M   $��24(�6Z   #��[ 59   
   ��58      �� *Localization[ExitAppButton]+��59   3   �� *ExitCommand+5:      5;      �� 5>      $��20q�6   .@�5?      �� ��6   �� *" Localization[BookingRequestsTab]+.��5@      � 	 � RowDefinitions	 5A      �� 5B      $
��Auto �6,   �� 5C      $��* �6,   �� 5D      $
��Auto �6,   �� 5E      $��* �6,   .+�5I      �� $��16(�5J   $   $
��Bold"�62   $��10q�5K   $     
Text1Brush#���  60   ��5I   $   �� *Localization[PendingRequests]+5K   0   �� 5M      $��1��6'   $
��Auto=�64   #���  6W   .��5N      �� $��10q�6S   ��6+   �� *PendingBookings+��5O   "   �� 5P   &   .R�5Q   *   ��   	TextBrush#���  61   $��1q�6[   $��0,0,0,10q�6o   
 �� CornerRadius$
 5��5R   1    �� Padding$ 10q�6B     LightBackgroundBrush#���  5S   1   ._�5T   .   �� .+�5U   2   �� $
��Bold"�6X   $��14(�6j   ��6<   �� *
Place.Name+6j   �� 5V   2   $��Italic$�6[   ��6<   �� *User.Username+6[   �� 5W   2   ��6<   �� *SelectedDate+ �� StringFormat$  {0:dd.MM.yyyy}���� 5X   2   �� 5Y   6   �6:   �� *Localization[Time]+ �� 5Z   6   �6:   �� *	StartTime+$ 
{0:hh\:mm}�� �� 5[   6   $	� - ��6:    �� 5\   6   �6:   �� *	EndTime+$ 
{0:hh\:mm}��5]   3   �� 5^   2   ��6<   �� *
TotalPrice+$ Сумма: {0} руб.���� 5`   2   $ 
Horizontal=�6=   $��Right=�6V   $��0,10,0,0q�6r   .+�5a   6   �� $��Принять��6=   $	��100��6P   $��0,0,10,0q�6\   
�������$��18(�6�     ApproveButtonStyle#��[ 5d   =   ��5b   =   �� *#!DataContext.ApproveBookingCommand+ �� RelativeSource ��  �� AncestorType ItemsControl��5c   =   �� *Id+5d   =   �� 5e   6   $��Отклонить��6=   $	��100��6Q   
�������$��18(�6p     RejectButtonStyle#��[ 5h   =   ��5f   =   �� *" DataContext.RejectBookingCommand+ ��  ItemsControl��5g   =   �� *Id+5h   =   5i   3   5j   /   5k   +   5l   '   5n      5o      �� 5r      $��16(�5s   $   $
��Bold"�62   $��10q�5t   $   $��2��60   
���   ���5r   $   �� *Localization[ActiveBookings]+5t   =   �� 5v      $��3��6'   $
��Auto=�64   #���  6W   .��5w      �� $��10q�6R   ��6+   �� *ActiveBookings+��5x   "   �� 5y   &   .R�5z   *   �� #���  61   $��1q�6[   $��0,0,0,10q�6o   $
 5��5{   1   $ 10q�6B   #���  5|   1   ._�5}   .   �� .+�5~   2   �� $
��Bold"�6X   $��14(�6j   
���   ���6<   �� *
Place.Name+6x   �� 5   2   $��Italic$�6[   ��6<   �� *User.Username+6[   �� 5�   2   ��6<   �� *SelectedDate+$  {0:dd.MM.yyyy}���� 5�   2   �� 5�   6   �6:   �� *Localization[Time]+ �� 5�   6   �6:   �� *	StartTime+$ 
{0:hh\:mm}�� �� 5�   6   $	� - ��6:    �� 5�   6   �6:   �� *	EndTime+$ 
{0:hh\:mm}��5�   3   �� 5�   2   ��6<   �� *
TotalPrice+$ Сумма: {0} руб.���� 5�   2   #���  6T   $��0,5,0,0q�6}   $
��Bold"�6�   $
��Left=�5�   >   $
��Auto��5�       �� TextAlignment$ Center=�5�      ��5�   <   �� *Status+��6�   �� *Status+ �� 	Converter# [ 5�      �� 5�   2   $ 
Horizontal=�6=   $��Right=�6V   $��0,10,0,0q�6r   .+�5�   6   �� $��Завершить��6=   $	��100��6Q   $��0,0,10,0q�6]   
�������$��18(�6�     CompleteButtonStyle#��[ 5�   !   ��5�   !   �� *$"DataContext.CompleteBookingCommand+ ��  ItemsControl��5�   !   �� *Id+y�5�   !   �� *Status+# [ �� 5�   6   $��Отклонить��6=   $	��100��6R   
�������$��18(�6q     CancelButtonStyle#��[ 5�   !   ��5�   !   �� *" DataContext.CancelBookingCommand+ ��  ItemsControl��5�   !   �� *Id+y�5�   !   �� *Status+# [ 5�   /   5�   /   5�   +   5�   '   5�      5�      5�      5�      �� 5�      ��6   �� *!Localization[UserManagementTab]+.��5�      �  � ColumnDefinitions 5�      �� 5�      $	��300 �6/   �� 5�      $��* �6/   .+�5�      �� $��0��6!   
�������$��0,0,1,0q�6D   ._�5�      �� $
��Auto=�6+   .��5�   "   �� $��10q�6M     PointerCursor � Cursor# �  6Y   ��6/   �� *Users+��5�   &   �� 5�   *   .R�5�   .   �� 
�������$��1q�6M   $��0,0,0,10q�6a   $
 5��5�   5   $ 10q�6F   ��5�   5   �� *	IsBlocked+# [ ._�5�   2   �� .+�5�   6   �� $
��Bold"�6Z   ��6@   �� *
Username+6Z   �� 5�   6   ��6@   �� *Email+�� 5�   6   $
��Bold"�5�   @   ��5�   @   �� *	IsBlocked+# [  �� ConverterParameter$- 'Заблокирован|Активен��5�   @   5�   3    J� InputBindings 5�   2   d� 5�   6    d� MouseAction$ 	LeftClicke�6C    �� Command 5�   C   �� *DataContext.SelectUserCommand+ ��  ItemsControl �� CommandParameter 5�   C   �� 5�   /   5�   +   5�   #   5�      5�      �� 5�      $��1��6!   $��10q�61   #���  6�   y�6=   �� *SelectedUser+# [  ._�5�      �� $
��Auto=�6+   .��5�   "   �� $��20q�6-   .+�5�   &   �� $	
 100��6.   $	��150��6A   $	��150��6M   $��Center=�5�      $	��Top=�62   #���  5�      ._�5�   *   �� $��UniformToFill=�6[   ��60   �� *SelectedUser.AvatarPath+ J� Clip 5�   .   @� 5�   2    @� Center$ 75,75&�6B    @� RadiusX$ 75Q�6Q    @� RadiusY$ 75Q�6^   5�   +   5�   '   �� 5�   &     EditProfileButtonStyle#��[ 5�      $��0,15,0,0q�5�      ��5�   -   �� *" Localization[ChangeAvatarButton]+��5�      �� *ChangeUserAvatarCommand+5�      �� 5�   &   $��0,20,0,0q�62   y�6D   �� *IsUserEditMode+# [ .+�5�   *   �� $��0,0,0,5q�6a   ��64   �� *Localization[UsernameLabel]+6a   �� 5�   *    �� 
IsReadOnly$
 True��6Y     AuthTextBoxStyle#��[ 6k   ��62   �� *SelectedUser.Username+6k   �� 5�   *   $��0,10,0,5q�6^   ��64   �� *Localization[EmailLabel]+6^   �� 5�   *   $
 True��6V   #��[ 6h   ��62   �� *SelectedUser.Email+6h   �� 5�   *   $��0,10,0,5q�6^   ��64   �� *Localization[PhoneLabel]+6^   �� 5�   *   $
 True��6V   #��[ 6h   ��62   �� *SelectedUser.Phone+6h   �� 5�   *   $��0,10,0,5q�6_   ��64   �� *Localization[StatusLabel]+6_   �� 5�   *   $
 True��5�      #��[ 6,   ��5�   2   �� *SelectedUser.IsBlocked+# [ $- 'Заблокирован|Активен��5�   ,   �� 5�   *   $ 
Horizontal=�65   $��Center=�6N   $
��0,20q�6k   .+�5�   .   �� $��18(�6b     EditButtonStyle#��[ 5�      $	��140��5�      $��0,0,10,0q�6   ��5�   5   �� *Localization[EditButton]+��5�      �� *EditUserCommand+5�      �� 5�   .   $��18(�5�   V     BlockButtonStyle#��[ 5�      $	��180��5�      ��5�   5   �� *SelectedUser.IsBlocked+# [ $= 7Разблокировать|Заблокировать����5�      �� *ToggleUserBlockCommand+5�      5�   +   5�   '   �� 5�   &   $��0,20,0,0q�61   y�6C   �� *IsUserEditMode+# [ .+�5�   *   �� $��0,0,0,5q�6a   ��64   �� *Localization[UsernameLabel]+6a   �� 5�   *   #��[ 6]   ��62   �� *TempUsername+ �� Mode$ TwoWay=�6]   �� 5�   *   $��0,10,0,5q�6^   ��64   �� *Localization[EmailLabel]+6^   �� 5�   *   #��[ 6Z   ��62   �� *	TempEmail+$ TwoWay=�6Z   �� 5   *   $��0,10,0,5q�6^   ��64   �� *Localization[PhoneLabel]+6^   �� 5  *   #��[ 6Z   ��62   �� *	TempPhone+$ TwoWay=�6Z   �� 5  *   $ 
Horizontal=�65   $��Center=�6N   $
��0,20q�6k   .+�5  .   �� $	��125��6b   $��0,0,10,0q�6n   $��24(�6�     SaveButtonStyle#��[ 5     ��5  5   �� *Localization[SaveButton]+��5     �� *SaveUserChangesCommand+5     �� 5  .   $	��120��6h   $��24(�6t   #��[ 5
     ��5  5   �� * Localization[CancelEditButton]+��5	     �� *CancelUserEditCommand+5
     5  +   5  '   5  #   5     5     5     5     5     5     �� 5  
   $ 
Horizontal=�6   $
��Left=�6.   $��Bottom=�6I   .+�5     ��   FlagButtonStyle#��[ 6@   $��	20,0,0,20q�6i   ��6   �� *SwitchToRussianCommand+.��5     ��   RussianFlag#��[ 6   $��24��6>   $��16��6I   5     �� 5     #��[ 6@   $��	20,0,0,20q�6i   ��6   �� *SwitchToEnglishCommand+.��5     ��   EnglishFlag#��[ 6   $��24��6>   $��16��6I   5     �� 5     $��	20,0,0,20q�5     ��5     �� *ToggleThemeCommand+ � ToolTip 5     �� *" Localization[ToggleThemeTooltip]+.��5      �� $��24��5!     $��24��6#   ��5      �� *IsDarkTheme+# [ 5!  #   5"     5#     5$     5%     
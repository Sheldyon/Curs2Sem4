   M S B A M L   `   `   `  ����F  BCyberHeaven, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:CyberHeaven.ViewsCyberHeaven.Views  5   	   ;"clr-namespace:CyberHeaven.CommandsCyberHeaven.Commands  5      ;"clr-namespace:CyberHeaven.ControlsCyberHeaven.Controls  5	      ?$clr-namespace:CyberHeaven.ViewModelsCyberHeaven.ViewModels  5
   	   ?$clr-namespace:CyberHeaven.ConvertersCyberHeaven.Converters  6A   "    CyberHeaven.Views.AuthWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:CyberHeaven.Views     5   	   7commands"clr-namespace:CyberHeaven.Commands     5      7controls"clr-namespace:CyberHeaven.Controls     5	      ;
viewmodels$clr-namespace:CyberHeaven.ViewModels     5
   	   ;
converters$clr-namespace:CyberHeaven.Converters     6A     � Icon$7  1D:\ООП\CyberHeaven\CyberHeaven\images\icon.ico��5   
    � Title$ 
AuthWindow��5   	    � WindowStyle$
 None=�5   	    � WindowState$ 	Maximized=�5   	    � AllowsTransparency$
 True��5   	   
������  � 	Resources 5       �� MergedDictionaries �� Source6   0CyberHeaven.Converters.NullToVisibilityConverter   NullToVisibilityConverter0   *CyberHeaven.Converters.NullToTextConverter  NullToTextConverter1   +CyberHeaven.Converters.ThemeToIconConverter  ThemeToIconConverter   BooleanToVisibilityConverter�� 5       5   
   �� 5      $ /Resources/Strings.xaml?�6!   %t   &	        &	      &	 &     &	 9       5   
   6/     5   
   6)     5   
   6*   �� 5   
   6'   5       J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5    
   -   5        	   Command$&	  commands:CustomCommands.OpenHelp��5       6D     5!   
   -   5        $)	 #commands:CustomCommands.RefreshData��5!      6G   
 J� InputBindings
 5#      �� 5$   
    �� Command$&  commands:CustomCommands.OpenHelp��6   
 �� Key$ F1��6@   �� 5%   
   $) #commands:CustomCommands.RefreshData��6   $ R��6C    �� 	Modifiers$ Controlf�6K   .��5(      � .+�5*   
   r� -   5        $��BackgroundGif��5*       r� Source$@ :D:\ООП\CyberHeaven\CyberHeaven\images\BackGroundVid.mp4?�5+       r� LoadedBehavior$
 Play=�5,       r� UnloadedBehavior$
 Stop=�5-       r� Stretch$ UniformToFill=�5.       r� IsMuted$
 True��5/       J� Opacity$	 100Q�50      �� 53   
   ���	#800000006   �� 56   
     BackgroundBrush#���  6    �� CornerRadius$ 20��57        PrimaryBrush#���  58      $��2q�59      $	��400��5:      $	��450��5;      $��Center=�5<      $��Center=�5=      ._�5>      � $��20q�6    � RowDefinitions 5?      �� 5@      $
��Auto �6$   �� 5A      $��* �6$   �� 5B      $
��Auto �6$   .+�5F      �� #���  5G      $��28(�5H      $
��Bold"�5I      $��Center=�5J      $��	0,10,0,20q�5K      ��5F      �� *" Localization[AuthorizationTitle]+5K      �� 5N      $��1��6   $��Center=�6*   .+�5P      ��   PrimaryDarkerBrush#���  6N   $	��0,5q�6�   ��6    �� *Localization[UsernameLabel]+6�   �� 5Q        AuthTextBoxStyle#��[ 5R      ��5Q      �� *
Username+ �� UpdateSourceTrigger$ PropertyChanged=�5R      �� 5S        ErrorTextStyle#��[ 6?   ��6    �� *UsernameError+6?   �� 5V      #���  6N   $	��0,5q�6�   ��6    �� *Localization[PasswordLabel]+6�   /   )CyberHeaven.Controls.PasswordInputControl  5W      /   )CyberHeaven.Controls.PasswordInputControl   PasswordChanged$* $PasswordInputControl_PasswordChanged��5Y         PasswordStrengthChecked$2 ,PasswordInputControl_PasswordStrengthChecked��5Z      !   PasswordVisibilityToggling$5 /PasswordInputControl_PasswordVisibilityToggling��5[          PasswordVisibilityToggled$4 .PasswordInputControl_PasswordVisibilityToggled��5\         ShowStrengthIndicator$
 True��5X         Password 5W   4   �� *
Password+ �� Mode$ TwoWay=�5\      �� 5_      #��[ 5`       ��5_       �� *	AuthError+y�5a       �� *HasAuthError+ �� 	Converter# [ 5b      �� 5e      $��2��6    �� Orientation$ Vertical=�6*   .+�5f      ��  	 AuthButtonStyle#��[	 5h      $��0,0,0,10q�5i      ��5f      �� *Localization[LoginButton]+��5g      �� *LoginCommand+5i      �� 5j       
 RegisterLinkButtonStyle#��[
 5l      ��5j      �� *Localization[RegisterPrompt]+��5k      �� *NavigateToRegisterCommand+5l      5m      �� 5n      $ 
Horizontal=�6   $��Right=�67   $��0,10,0,0q�6S   .+�5o      ��   FlagButtonStyle#��[ 6H   ��6   �� *SwitchToRussianCommand+.��5p      ��   RussianFlag#��[ 6    $��24��6F   $��16��6Q   5q      �� 5r      #��[ 6H   $��10,0,0,0q�6q   ��6   �� *SwitchToEnglishCommand+.��5s      ��   EnglishFlag#��[ 6    $��24��6F   $��16��6Q   5t      5u      �� 5v      $ 
Horizontal=�6   $
��Left=�66   $��0,10,0,0q�6Q   .+�5w      �� $��10,0,0,5q�5x      ��5w      �� *ToggleThemeCommand+  � ToolTip  5y      �� *" Localization[ToggleThemeTooltip]+��5z      �� 5{      ! �� 
TargetType"! ��6$     ThemeToggleButtonStyle" �� BasedOn#" [ 68   .�5|   "   �� # �� Property# �@�Cursor  DefaultCursor$ �� Value#$ �  6;   % �� Triggers% 5}   "   P� 5~   &   & P� Property& �@J�IsMouseOver' P� Value$
' True��6E   . �5   *   �� # �@�Cursor  PointerCursor#$ �  6C   5�   '   5�      .��5�      �� $��24��5�      $��24��6   ��5�       �� *IsDarkTheme+# [ 5�      5�      5�      �� 5�      $&�� commands:CustomCommands.OpenHelp��5�      $��24(�5�      $��10,10,700,10q�5�      $��Bottom=�5�      $	��300��5�        BaseButtonStyle#��[ 5�      ��5�      �� *Localization[HelpButton]+5�      5�      5�      �� 5�   
   $��24(�6>   $��10,10,1000,10q�6L   $��Bottom=�6c   $	��300��6~   #��[ 5�      ��5�      �� *Localization[MainButton]+��5�   =   �� *OpenMainCommand+�� 5�   
   $��24(�6A   $��10,10,-1000,10q�6O   $��Bottom=�6g   $	��300��6�   #��[ 5�      ��5�      �� *Localization[ExitAppButton]+��5�   *   �� *ExitCommand+5�      5�      
   M S B A M L   `   `   `  ����M  IDanceScore_pontozo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:DanceScore_pontozoDanceScore_pontozo  5   	   !    DanceScore_pontozo.Pontozas   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:DanceScore_pontozo     5   	     � Title$  Pontozas��5   	   $	��450��6   $	��900��6'   .��5
      � ��5   
   �� 5       �� 
StartPoint$	 0,0&�6"    �� EndPoint$	 0,1&�63   .��5      �  � Color$ White��6    � Offset$ 0Q�6-   � 5      $ 	#FF76C0C0��6   $ 1Q�61   5       � ColumnDefinitions 5   
   �� 5      $��1* �6   �� 5      $��2* �6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��6* �6   .+�5   
   �� -   5         ��Name$ welcomeText��5      $;��5Üdvözöljük a Dance Score pontozórendszerünkben!��5      $��26(�5      $
��Bold"�6"   $��Gabriola)�5      $��Center=�5       $��Center=�61    J� Opacity$ 0Q�5!      ���	#FF4A7070	 J� RenderTransformOrigin$	 0.5, 0.5&�5#      $��0��5$      $��2��6!   z�5%      T� 5&      
 T� Y$
 20Q�6%    � Triggers 5)      :� 5*       :� RoutedEvent$ FrameworkElement.Loaded��6   .[�5+      �� .��5,      �� .��5-      W�  �� TargetProperty$ Opacity�6.    W� From$ 0X�5.   .   	 W� To$ 1X�67    g� Duration$ 0:0:2.5D�6>   W� 5/      $, &RenderTransform.(TranslateTransform.Y)�6.   $ 20X�50   .   $ 0X�68   $ 0:0:2.5D�6?   �� 51      $4 .(TextBlock.Foreground).(SolidColorBrush.Color)�6-    �� From$ 	#FF4A7070X�52   -   	 �� To$ 	#FF1E4D4DX�6>   $ 0:0:2.5D�53   -   54      55      56      58      �� 5;   
   $��1��6   $��0��6"   $��Center=�62   .+�5=      �� -   5        $ selectCompetitionText��5=      $&�� Kérem, válassza ki a versenyt!��5>      $��22(�5?      $��Gabriola)�5@      $
��Bold"�5A      $��Center=�5B      ���	#FF4A7070$��0,0,0,10q�5D      �� 5F      .+�5G      �� $��Verseny:��6   $��18(�6+   $��Gabriola)�69   $
��Bold"�6O   $
��Left=�6a   ���	#FF4A7070$��30��6�   �� 5H      -   5         ��Name$ verseny��5H      6,   5I      �� 5K      .+�5L      �� $��Kategória:��6   $��18(�6-   $��Gabriola)�6;   $
��Bold"�6Q   $
��Left=�6c   ���	#FF4A7070$��30��6�   �� 5M      -   5        $ 	kategoria��5M      6.   5N      5P      �� 5R   
    �� Orientation$ 
Horizontal=�6   $
��Left=�6.   $��1��6I   $��1��6Y   .+�5T      �� $ Vertical=�6   .+�5U      �� $%��Kérem válassza ki a csapatot!��6   $��22(�6@   $��Gabriola)�6N   $
��Bold"�6d   $
��Left=�6v   ���	#FF4A7070$��35��6�   �� 5V      -   5         ��Name$ csapatok��5V      $
��Left=�6,   $	��220��6G   $	��310��6S   5W      5Y      �� 5[   
   $ 
Horizontal=�6   $��Right=�6.   $��1��6J   $��1��6Z   $	��298��6g   .+�5]      �� $ Vertical=�6   $��Center=�60   $��Center=�6M   .+�5_      �� $ 
Horizontal=�6   $��Right=�66   $��Center=�6R   $��50��6m   .+�5`      �� $��	Ruházat:��6   $��18(�6/   $��Gabriola)�6=   $
��Bold"�6S   $��Center=�6e   ���	#FF4A7070$��35��6�   �� 5a      -   5         ��Name$ ruhazat��5a       �� TextAlignment$ Center=�6/   $��Center=�6F   $��Center=�6c   $��40��6~   $��50��6�   5b      �� 5d      $ 
Horizontal=�6   $��Right=�66   $��Center=�6R   $��50��6m   .+�5e      �� $��	Szinkron:��6   $��18(�60   $��Gabriola)�6>   $
��Bold"�6T   $��Center=�6f   ���	#FF4A7070$��35��6�   �� 5f      -   5        $ szinkron��5f      $ Center=�60   $��Center=�6G   $��Center=�6d   $��40��6   $��50��6�   5g      �� 5i      $ 
Horizontal=�6   $��Right=�66   $��Center=�6R   $��50��6m   .+�5j      �� $��Művészi hatás:��6   $��18(�65   $��Gabriola)�6C   $
��Bold"�6Y   $��Center=�6k   ���	#FF4A7070$��35��6�   �� 5k      -   5        $ muvhatas��5k      $ Center=�60   $��Center=�6G   $��Center=�6d   $��40��6   $��50��6�   5l      �� 5n      $ 
Horizontal=�6   $��Right=�66   $��Center=�6R   $��50��6m   .+�5o      �� $��Előadásmód:��6   $��18(�62   $��Gabriola)�6@   $
��Bold"�6V   $��Center=�6h   ���	#FF4A7070$��35��6�   �� 5p      -	   5        $ 
eloadasmod��5p      $ Center=�62   $��Center=�6I   $��Center=�6f   $��40��6�   $��50��6�   5q      �� 5s      $ 
Horizontal=�6   $��Right=�66   $��Center=�6R   $��50��6m   .+�5t      �� $��	Technika:��6   $��18(�60   $��Gabriola)�6>   $
��Bold"�6T   $��Center=�6f   ���	#FF4A7070$��35��6�   �� 5u      -
   5        $ technika��5u      $ Center=�60   $��Center=�6G   $��Center=�6d   $��40��6   $��50��6�   5v      5x      5z      �� 5|   
   $��Right=�6   $��1��61   $��1��6A   $��Center=�6N   $	��100��6i   .+�5}      �� -   5         ��Name$ bekuld��5}      $��Beküld��6%   $
��Left=�66   $��Center=�6Q   $��50��6l   6w   5~      5�      5�      
   M S B A M L   `   `   `  ����  WpfApplication2    WpfApplication2.Page1   5      \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5        N� Title$  Page1��5      .��5      ]�  � 	Resources 5         buttonStyle �� Property �� Value  topButtonStyle��%�  &	        &	 �     �� 5      .�5   
   ��  � ��$ Tahoma)�6.   �� 5	   
    � ��$ 14(�6.   �� 5
   
    � ��$
 Bold"�6.   �� 5   
    � ��
 �����6.   5   	   �� 5      .�5   
   ��  � ��$ Tahoma)�6.   �� 5   
    � ��$ 12(�6.   �� 5   
    � ��$
 Bold"�6.   �� 5   
    � �� �#eeeeee6.   �� 5   
    � ��$ 10q�6.   5   	   5      .+�5      �� ���#eeeeee$��75��6&   $	��Top=�62   .+�5      ]� .+�5   
   �� -   5        #��[ 5      $��Right=�69   $��1q�6e   ���	#FFFFEEEE.��6�   Help6�   �� 5   
    �� Orientation$ 
Horizontal=�6   .+�5      �� $��Tahoma)�6    $��20(�64   
����� �$
��bold"�6[    �� Padding$ 10q�6m   Key/Value Database6�   �� 5      -   5        $��	PageLabel��5      $��Center=�6#   $��	30,5,30,5q�6>   $	��304��6R   $��14(�6^   .��6l   Client6t   5      5   	   o� 5      
����� �.@�5      m� -   5        $��Database Connection��5      
�������$	��250��6g   $��	50,5,50,5q�6s   $��Center=�6�   
���iii�6�   m� 5       -   5        $��Insert/Edit/Delete Value ��5       
�������$	��250��6o   $��	50,5,50,5q�6{   $��Center=�6�   m� 5!      -   5        $��      Query Database��5!      
�������$��	50,5,50,5q�6P   $	��250��6d   $��Center=�6�   m� 5"      -   5        $��      Persist Database��5"      
�������$��	50,5,50,5q�6R   $	��250��6f   $��Center=�6�   5%   	   5&      �� 5(   
   
����� �$��25��6.   $ 
Horizontal=�6:   $��Bottom=�6S   .+�5)      �� $#��Status: Connected to database��6   
�������66   5*      �� 5+   
   
�������$	��300��6(   $ 
Horizontal=�65   .+�5,      �� 
�������$	��300��6,   $	��450��69   $ Vertical=�6E   .+�5-      �� -   5         ��Name$ textBox1��5-      $��Tahoma)�6.   $��15(�6B   $
��Wrap=�6P   $0��*Please fill the server details and connect��6d    �� TextAlignment$ Center=�5.      $ 25q�61   �� 5/      
�������$ 
Horizontal=�62   $��0,35,0,0q�6K   .+�50      �� -   5        $ textBox2��50       $��Tahoma)�62   $��15(�6G   $
��Wrap=�6U   $��Server:��6i   $ Center=�51      $	��100��61   $ 10q�6=   $��
100,0,10,0q�6J   �� 52      -	   5         ��Name$ 	textBlock��52      $��Tahoma)�61   $��15(�6E   	 �� TextWrapping$
	 Wrap=�6T   $��localhost:9030��6h   
 �� TextAlignment$
 Center=�53      $	��150��61   $��25��6=   |�. 6I   54      �� 55      -
   5         ��Name$ button��55      $��Connect��6)   $��30��6;   $	��100��6G   $��0,20,0,0q�6T   
�������6}   57      �� 58      -   5        $��Canvas1��58      $	��300��6%   $��Right=�62   $	��600��6I   .+�59      �� -   5        $��Request��59   4   $ Vertical=�6   $	��300��6C   
 �� Top$ 0��6P    �� Left$ 0��6_    K� ZIndex$ 1��6o   .+�5:      �� $��Tahoma)�6    $��Center=�64   $��15(�6Q   $
��bold"�6_   $ 10q�6q   Requested Field6�   �� 5;      -   5        $��listBox1��5;   A   $��1��6   $��10, 0, 10, 0q�6+   $	��250��6Q   .@�5<      �� .��6&   0.Requesting to connect to server localhost:90306V   5=      5?      �� 5@      -   5        $��Responce��5@   4   $ Vertical=�6   $	��300��6D   $ 0��6Q   $	 310��6`   $ 2��6r   .+�5A      �� $��Tahoma)�6    $��Center=�64   $��15(�6Q   $
��bold"�6_   $ 10q�6q   Response Field6�   �� 5B      -   5        $��listBox3��5B   A   $��1��6   $��10, 0, 10, 0q�6+   $	��250��6Q   .@�5C      �� .��6&   $"Connected to server localhost:90306J   5D      5E      5F      5G      5H      5I      
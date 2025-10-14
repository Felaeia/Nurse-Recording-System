import 'package:flutter/material.dart';

class AppColors {

  static const LinearGradient primaryGradient = LinearGradient(
    colors: [Color(0xFF2933FF), Color(0xFFFF5451)],
    begin: Alignment.topLeft,
    end: Alignment.bottomRight,
  );

  static const LinearGradient fieldGradient = LinearGradient(
    colors: [
      Color.fromARGB(255, 91, 132, 255),
      Color.fromARGB(228, 255, 188, 188),
    ],
    begin: Alignment.centerLeft,
    end: Alignment.centerRight,
  );

  static const Color accentRed = Color(0xFFFF5451);
  static const Color primaryBlue = Color(0xFF2933FF);
  static const Color lightGrey = Color(0xFFBDBDBD);
  static const Color black = Color.fromARGB(255, 0, 0, 0);
}

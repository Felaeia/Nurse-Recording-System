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
}

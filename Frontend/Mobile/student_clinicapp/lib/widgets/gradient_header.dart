import 'package:flutter/material.dart';
import './app_colors.dart';
import 'app_text_styles.dart';

class GradientHeader extends StatelessWidget {
  final String title;

  const GradientHeader({super.key, required this.title});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(vertical: 40, horizontal: 20),
      width: double.infinity,
      decoration: const BoxDecoration(
        gradient: AppColors.primaryGradient,
        borderRadius: BorderRadius.only(
          bottomLeft: Radius.circular(24),
          bottomRight: Radius.circular(24),
        ),
      ),
      child: Column(
        children: [
          const CircleAvatar(
            radius: 50,
            backgroundColor: Colors.white24,
            child: Icon(Icons.person, size: 60, color: Colors.white),
          ),
          const SizedBox(height: 12),
          Text(title, style: AppTextStyles.header),
        ],
      ),
    );
  }
}

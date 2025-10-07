import 'dart:ui';
import 'package:flutter/material.dart';
import '../widgets/app_colors.dart';
import '../widgets/app_background.dart';

class UserInfo extends StatelessWidget {
  const UserInfo({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: AppBackground(
        child: Column(
          children: [
            Container(
              padding: const EdgeInsets.symmetric(vertical: 40, horizontal: 20),
              width: double.infinity,
              decoration: const BoxDecoration(
                gradient: AppColors.primaryGradient,
                borderRadius: BorderRadius.only(
                  bottomLeft: Radius.circular(24),
                  bottomRight: Radius.circular(24),
                ),
              ),
              child: const Column(
                children: [
                  CircleAvatar(
                    radius: 50,
                    backgroundColor: Colors.white24,
                    child: Icon(Icons.person, size: 60, color: Colors.white),
                  ),
                  SizedBox(height: 12),
                  Text(
                    "User Information",
                    style: TextStyle(
                      fontSize: 22,
                      fontWeight: FontWeight.bold,
                      color: Colors.white,
                    ),
                  ),
                ],
              ),
            ),
            Expanded(
              child: ListView(
                padding: const EdgeInsets.all(20),
                children: const [
                  _GradientInfoTile(label: "Name", value: "Ayumi Fukaishi"),
                  _GradientInfoTile(label: "Age", value: "20"),
                  _GradientInfoTile(label: "Student ID", value: "2025-12345"),
                  _GradientInfoTile(label: "Email", value: "ayumi@example.com"),
                  _GradientInfoTile(label: "Phone", value: "+63 912 345 6789"),
                  _GradientInfoTile(label: "Address", value: "Cebu City, Philippines"),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}

class _GradientInfoTile extends StatelessWidget {
  final String label;
  final String value;
  const _GradientInfoTile({required this.label, required this.value});

  @override
  Widget build(BuildContext context) {
    return Container(
      margin: const EdgeInsets.symmetric(vertical: 8),
      padding: const EdgeInsets.all(2),
      decoration: BoxDecoration(
        gradient: AppColors.fieldGradient,
        borderRadius: BorderRadius.circular(12),
      ),
      child: Container(
        padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 14),
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(10),
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              label,
              style: const TextStyle(
                fontWeight: FontWeight.bold,
                color: Colors.black54,
                fontSize: 14,
              ),
            ),
            const SizedBox(height: 4),
            Text(
              value,
              style: const TextStyle(
                fontWeight: FontWeight.w500,
                fontSize: 16,
                color: Colors.black87,
              ),
            ),
          ],
        ),
      ),
    );
  }
}

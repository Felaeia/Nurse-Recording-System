import 'dart:ui';
import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:student_clinicapp/landing/signin.dart';
import 'package:student_clinicapp/screens/userinfo.dart';
import '../widgets/app_background.dart';
import '../widgets/app_colors.dart';

class Medication {
  final String name;
  final String date;
  const Medication(this.name, this.date);
}

void main() {
  runApp(const MaterialApp(
    debugShowCheckedModeBanner: false,
    home: Home(),
  ));
}

class Home extends StatefulWidget {
  const Home({super.key});
  @override
  State<Home> createState() => _HomeState();
}

class _HomeState extends State<Home> {
  final List<Medication> meds = const [
    Medication("Paracetamol 500mg", "2025-08-10"),
    Medication("Ibuprofen 200mg", "2025-07-22"),
    Medication("Vitamin C 1000mg", "2025-06-15"),
    Medication("Amoxicillin 250mg", "2025-05-05"),
  ];

  final List<String> allRecords = const [
    "Radio XRay",
    "Lung XRay",
    "Blood Test",
    "Abdomen XRay",
    "Spine CT Scan",
    "Brain CT Scan",
    "Heart Scan",
    "Chest MRI",
  ];

  Offset fabPosition = const Offset(320, 600);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: AppBackground(
        child: Stack(
          children: [
            SafeArea(
              child: Padding(
                padding: const EdgeInsets.symmetric(horizontal: 24, vertical: 16),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const SizedBox(height: 12),
                    const _Header(),
                    const SizedBox(height: 32),
                    _MedicationSection(meds: meds),
                    const SizedBox(height: 24),
                    Expanded(child: _PatientRecordSection(allRecords: allRecords)),
                  ],
                ),
              ),
            ),
            Positioned(
              left: fabPosition.dx,
              top: fabPosition.dy,
              child: Draggable(
                feedback: Material(
                  color: Colors.transparent,
                  child: const FloatingChatIcon(),
                ),
                childWhenDragging: const SizedBox.shrink(),
                onDragEnd: (details) {
                  setState(() {
                    final size = MediaQuery.of(context).size;
                    double x = details.offset.dx.clamp(0.0, size.width - 72);
                    double y = details.offset.dy.clamp(0.0, size.height - 72);
                    fabPosition = Offset(x, y);
                  });
                },
                child: const FloatingChatIcon(),
              ),
            ),
          ],
        ),
      ),
    );
  }
}

class _Header extends StatelessWidget {
  const _Header();
  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Row(
          children: [
            SizedBox(
              height: 80,
              width: 80,
              child: ShaderMask(
                shaderCallback: (bounds) => AppColors.primaryGradient.createShader(bounds),
                blendMode: BlendMode.srcIn,
                child: SvgPicture.asset(
                  'assets/ACLC.svg',
                  fit: BoxFit.contain,
                  placeholderBuilder: (context) =>
                      const Icon(Icons.school, color: Colors.white, size: 48),
                ),
              ),
            ),
            const SizedBox(width: 12),
            ShaderMask(
              shaderCallback: (bounds) => AppColors.primaryGradient.createShader(bounds),
              child: const Text(
                "Hello, Ayums!",
                style: TextStyle(
                  fontSize: 25,
                  fontWeight: FontWeight.w900,
                  color: Colors.white,
                ),
              ),
            ),
          ],
        ),
        PopupMenuButton<String>(
          padding: EdgeInsets.zero,
          offset: const Offset(0, 40),
          color: Colors.white,
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
          child: Container(
            decoration: BoxDecoration(
              shape: BoxShape.circle,
              gradient: AppColors.primaryGradient,
            ),
            child: const CircleAvatar(
              radius: 20,
              backgroundColor: Colors.transparent,
              child: Icon(Icons.person, color: Colors.white),
            ),
          ),
          itemBuilder: (_) => [
            _menuItem('account', Icons.person, 'Account', Colors.grey[700]!),
            _menuItem('logout', Icons.logout, 'Logout', Colors.red),
          ],
          onSelected: (value) {
            if (value == 'account') {
              Navigator.push(
                context,
                MaterialPageRoute(builder: (_) => const UserInfo()),
              );
            } else if (value == 'logout') {
              Navigator.pushReplacement(
                context,
                MaterialPageRoute(builder: (_) => const SignIn()),
              );
            }
          },
        ),
      ],
    );
  }

  PopupMenuItem<String> _menuItem(String value, IconData icon, String text, Color color) {
    return PopupMenuItem(
      value: value,
      child: Row(
        children: [
          Icon(icon, color: color, size: 20),
          const SizedBox(width: 8),
          Text(
            text,
            style: TextStyle(color: color, fontWeight: FontWeight.w600),
          ),
        ],
      ),
    );
  }
}

class _MedicationSection extends StatelessWidget {
  final List<Medication> meds;
  const _MedicationSection({required this.meds});

  @override
  Widget build(BuildContext context) {
    final displayMeds = meds.take(3).toList();
    return _GradientGlassContainer(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: const [
              Icon(Icons.medication, size: 28, color: Colors.white),
              SizedBox(width: 12),
              Text(
                'Medication History',
                style: TextStyle(
                  color: Colors.white,
                  fontSize: 20,
                  fontWeight: FontWeight.bold,
                ),
              ),
            ],
          ),
          const SizedBox(height: 16),
          ...displayMeds.map((m) => _MedItem(m.name, m.date)),
          const SizedBox(height: 12),
          Align(
            alignment: Alignment.centerRight,
            child: _GradientBorderButton(
              text: 'View More',
              onPressed: () {},
              boldText: false,
            ),
          ),
        ],
      ),
    );
  }
}

class _PatientRecordSection extends StatelessWidget {
  final List<String> allRecords;
  const _PatientRecordSection({required this.allRecords});

  @override
  Widget build(BuildContext context) {
    return _GradientGlassContainer(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: const [
              Icon(Icons.folder, size: 28, color: Colors.white),
              SizedBox(width: 12),
              Text(
                'Patient Record',
                style: TextStyle(
                  color: Colors.white,
                  fontSize: 20,
                  fontWeight: FontWeight.bold,
                ),
              ),
            ],
          ),
          const SizedBox(height: 16),
          Expanded(
            child: ListView.builder(
              itemCount: allRecords.length,
              itemBuilder: (_, i) => _PatientRecordItem(title: allRecords[i]),
            ),
          ),
        ],
      ),
    );
  }
}

class _GradientBorderButton extends StatelessWidget {
  final String text;
  final VoidCallback onPressed;
  final bool boldText;
  const _GradientBorderButton({
    required this.text,
    required this.onPressed,
    this.boldText = true,
  });

  @override
  Widget build(BuildContext context) {
    return InkWell(
      onTap: onPressed,
      borderRadius: BorderRadius.circular(12),
      child: Container(
        padding: const EdgeInsets.all(2),
        decoration: BoxDecoration(
          gradient: AppColors.primaryGradient,
          borderRadius: BorderRadius.circular(12),
        ),
        child: Container(
          padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 6),
          decoration: BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.circular(10),
          ),
          child: ShaderMask(
            shaderCallback: (bounds) => AppColors.primaryGradient.createShader(bounds),
            child: Text(
              text,
              style: TextStyle(
                fontWeight: boldText ? FontWeight.bold : FontWeight.normal,
                color: Colors.white,
                fontSize: 13,
              ),
            ),
          ),
        ),
      ),
    );
  }
}

class FloatingChatIcon extends StatelessWidget {
  const FloatingChatIcon({super.key});
  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () {},
      child: Container(
        padding: const EdgeInsets.all(14),
        decoration: BoxDecoration(
          shape: BoxShape.circle,
          gradient: AppColors.primaryGradient,
          boxShadow: const [
            BoxShadow(
              color: Color.fromARGB(126, 32, 66, 124),
              blurRadius: 12,
              spreadRadius: 2,
            ),
          ],
        ),
        child: const Icon(Icons.chat_bubble_outline, size: 28, color: Colors.white),
      ),
    );
  }
}

class _GradientGlassContainer extends StatelessWidget {
  final Widget child;
  const _GradientGlassContainer({required this.child});

  @override
  Widget build(BuildContext context) {
    return ClipRRect(
      borderRadius: BorderRadius.circular(20),
      child: BackdropFilter(
        filter: ImageFilter.blur(sigmaX: 10, sigmaY: 10),
        child: Container(
          padding: const EdgeInsets.all(20),
          margin: const EdgeInsets.only(bottom: 20),
          decoration: BoxDecoration(
            gradient: AppColors.primaryGradient,
            borderRadius: BorderRadius.circular(20),
            border: Border.all(width: 2, color: Colors.white.withOpacity(0.3)),
            boxShadow: [
              BoxShadow(
                color: Colors.black.withOpacity(0.1),
                blurRadius: 8,
                offset: const Offset(0, 4),
              ),
            ],
          ),
          child: child,
        ),
      ),
    );
  }
}

class _MedItem extends StatelessWidget {
  final String name;
  final String date;
  const _MedItem(this.name, this.date);

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 6),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Text(name, style: const TextStyle(color: Colors.white, fontSize: 16)),
          Text(date, style: const TextStyle(color: Colors.white70, fontSize: 14)),
        ],
      ),
    );
  }
}

class _PatientRecordItem extends StatelessWidget {
  final String title;
  const _PatientRecordItem({required this.title});

  @override
  Widget build(BuildContext context) {
    return Container(
      margin: const EdgeInsets.symmetric(vertical: 6),
      padding: const EdgeInsets.all(12),
      decoration: BoxDecoration(
        color: Colors.white.withOpacity(0.10),
        borderRadius: BorderRadius.circular(8),
      ),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Text(title, style: const TextStyle(color: Colors.white, fontSize: 16)),
          _GradientBorderButton(
            text: 'Download PDF',
            onPressed: () => debugPrint('Downloading PDF...'),
            boldText: false,
          ),
        ],
      ),
    );
  }
}

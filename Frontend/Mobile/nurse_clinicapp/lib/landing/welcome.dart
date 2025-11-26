import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import '../landing/signin.dart';
import '../widgets/app_background.dart';

const _kGradient = LinearGradient(
  colors: [Color(0xFF2933FF), Color(0xFFFF5451)],
  begin: Alignment.topLeft,
  end: Alignment.bottomRight,
);

class Welcome extends StatefulWidget {
  const Welcome({super.key});

  @override
  State<Welcome> createState() => _WelcomeState();
}

class _WelcomeState extends State<Welcome> with TickerProviderStateMixin {
  late AnimationController _zoomCtrl, _floatCtrl, _exitCtrl;
  late Animation<double> _zoom, _float;
  late Animation<Offset> _slide;

  @override
  void initState() {
    super.initState();

    _zoomCtrl = AnimationController(vsync: this, duration: const Duration(milliseconds: 1500));
    _zoom = Tween(begin: 0.0, end: 1.0).animate(CurvedAnimation(parent: _zoomCtrl, curve: Curves.easeOutBack));
    _zoomCtrl.forward();

    _floatCtrl = AnimationController(vsync: this, duration: const Duration(milliseconds: 1500))..repeat(reverse: true);
    _float = Tween(begin: -10.0, end: 10.0).animate(CurvedAnimation(parent: _floatCtrl, curve: Curves.easeInOut));

    _exitCtrl = AnimationController(vsync: this, duration: const Duration(milliseconds: 800));
    _slide = Tween(begin: Offset.zero, end: const Offset(-2, 0))
        .animate(CurvedAnimation(parent: _exitCtrl, curve: Curves.easeInOut));

    Future.delayed(const Duration(seconds: 4), () => _exitCtrl.forward());
    _exitCtrl.addStatusListener((s) {
      if (s == AnimationStatus.completed) {
        Navigator.pushReplacement(context, MaterialPageRoute(builder: (_) => const SignIn()));
      }
    });
  }

  @override
  void dispose() {
    _zoomCtrl.dispose();
    _floatCtrl.dispose();
    _exitCtrl.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return AppBackground(
      child: Scaffold(
        backgroundColor: Colors.transparent,
        body: SafeArea(
          child: Center(
            child: SlideTransition(
              position: _slide,
              child: ScaleTransition(
                scale: _zoom,
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    AnimatedBuilder(
                      animation: _float,
                      builder: (_, child) => Transform.translate(
                        offset: Offset(0, _float.value),
                        child: ShaderMask(
                          shaderCallback: (bounds) => _kGradient.createShader(bounds),
                          child: SvgPicture.asset(
                            'assets/ACLC.svg',
                            width: 160,
                            height: 160,
                            colorFilter: const ColorFilter.mode(Colors.white, BlendMode.srcIn),
                          ),
                        ),
                      ),
                    ),
                    const SizedBox(height: 8),
                    ShaderMask(
                      shaderCallback: (bounds) => _kGradient.createShader(bounds),
                      child: const Text(
                        'ACLC Clinic',
                        style: TextStyle(
                          fontSize: 46,
                          fontWeight: FontWeight.w900,
                          letterSpacing: 2,
                          color: Colors.white,
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }
}

import 'package:flutter/material.dart';

// Old gradient colors
const LinearGradient kPrimaryGradient = LinearGradient(
  colors: [Color(0xFF2933FF), Color(0xFFFF5451)],
  begin: Alignment.topLeft,
  end: Alignment.bottomRight,
);

class CustomButton extends StatefulWidget {
  final String text;
  final VoidCallback onPressed;

  const CustomButton({super.key, required this.text, required this.onPressed});

  @override
  State<CustomButton> createState() => _CustomButtonState();
}

class _CustomButtonState extends State<CustomButton> {
  bool _pressed = false;

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTapDown: (_) => setState(() => _pressed = true),
      onTapUp: (_) {
        widget.onPressed();
        setState(() => _pressed = false);
      },
      onTapCancel: () => setState(() => _pressed = false),
      child: AnimatedContainer(
        duration: const Duration(milliseconds: 150),
        width: 280,
        height: 55,
        padding: const EdgeInsets.all(2),
        decoration: BoxDecoration(
          gradient: kPrimaryGradient,
          borderRadius: BorderRadius.circular(30),
        ),
        child: Container(
          decoration: BoxDecoration(
            color: _pressed ? null : Colors.white,
            gradient: _pressed ? kPrimaryGradient : null,
            borderRadius: BorderRadius.circular(30),
          ),
          child: Center(
            child: _pressed
                ? Text(
                    widget.text,
                    style: const TextStyle(
                      fontSize: 18,
                      fontWeight: FontWeight.bold,
                      letterSpacing: 1.5,
                      color: Colors.white,
                    ),
                  )
                : ShaderMask(
                    shaderCallback: (bounds) =>
                        kPrimaryGradient.createShader(bounds),
                    child: Text(
                      widget.text,
                      style: const TextStyle(
                        fontSize: 18,
                        fontWeight: FontWeight.bold,
                        letterSpacing: 1.5,
                        color: Colors.white,
                      ),
                    ),
                  ),
          ),
        ),
      ),
    );
  }
}

import 'package:flutter/material.dart';
import '../widgets/app_colors.dart';

class UserInfo extends StatefulWidget {
  const UserInfo({super.key});

  @override
  State<UserInfo> createState() => _UserInfoState();
}

class _UserInfoState extends State<UserInfo> {
  final _formKey = GlobalKey<FormState>();
  final _firstNameController = TextEditingController();
  final _middleNameController = TextEditingController();
  final _lastNameController = TextEditingController();
  final _addressController = TextEditingController();
  final _facebookController = TextEditingController();
  final _emailController = TextEditingController();
  final _phoneController = TextEditingController();
  final _passwordController = TextEditingController();
  final _rePasswordController = TextEditingController();

  bool _passwordVisible = false;
  bool _rePasswordVisible = false;

  @override
  void dispose() {
    for (final c in [
      _firstNameController,
      _middleNameController,
      _lastNameController,
      _addressController,
      _facebookController,
      _emailController,
      _phoneController,
      _passwordController,
      _rePasswordController,
    ]) {
      c.dispose();
    }
    super.dispose();
  }

  void _submitForm() {
    if (_formKey.currentState?.validate() ?? false) {
      if (_passwordController.text != _rePasswordController.text) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(
            content: Text("Passwords do not match"),
            backgroundColor: Colors.redAccent,
          ),
        );
        return;
      }
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text("Profile Updated!"),
          backgroundColor: Colors.green,
        ),
      );
    }
  }

  Widget _animatedTextField(
    TextEditingController controller,
    String label,
    IconData icon, {
    bool requiredField = true,
    bool isPassword = false,
    bool passwordVisible = false,
    VoidCallback? togglePassword,
    int maxLines = 1,
    TextInputType keyboard = TextInputType.text,
  }) {
    final focusNode = FocusNode();

    return StatefulBuilder(
      builder: (context, setState) {
        focusNode.addListener(() => setState(() {}));
        return AnimatedContainer(
          duration: const Duration(milliseconds: 250),
          decoration: BoxDecoration(
            gradient: focusNode.hasFocus
                ? AppColors.primaryGradient
                : const LinearGradient(colors: [Colors.white, Colors.white]),
            borderRadius: BorderRadius.circular(15),
          ),
          padding: const EdgeInsets.all(1.5),
          child: Container(
            decoration: BoxDecoration(
              color: Colors.white,
              borderRadius: BorderRadius.circular(14),
            ),
            child: TextFormField(
              controller: controller,
              focusNode: focusNode,
              obscureText: isPassword ? !passwordVisible : false,
              maxLines: maxLines,
              keyboardType: keyboard,
              validator: (value) =>
                  requiredField && (value == null || value.isEmpty)
                      ? "$label is required"
                      : null,
              decoration: InputDecoration(
                labelText: label,
                prefixIcon: Icon(icon,
                    color: focusNode.hasFocus
                        ? AppColors.primaryGradient.colors.first
                        : Colors.grey.shade600),
                suffixIcon: isPassword
                    ? IconButton(
                        icon: Icon(
                          passwordVisible
                              ? Icons.visibility
                              : Icons.visibility_off,
                          color: Colors.grey.shade600,
                        ),
                        onPressed: togglePassword,
                      )
                    : null,
                labelStyle: TextStyle(
                    color: Colors.grey.shade600, fontWeight: FontWeight.w500),
                border: InputBorder.none,
                contentPadding:
                    const EdgeInsets.symmetric(vertical: 18, horizontal: 16),
              ),
            ),
          ),
        );
      },
    );
  }

  Widget _buildSection({
    required String title,
    required IconData icon,
    required List<Widget> children,
  }) {
    return Container(
      margin: const EdgeInsets.symmetric(vertical: 12),
      padding: const EdgeInsets.all(24),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(20),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Container(
                padding: const EdgeInsets.all(10),
                decoration: BoxDecoration(
                  gradient: AppColors.primaryGradient,
                  borderRadius: BorderRadius.circular(12),
                ),
                child: Icon(icon, color: Colors.white, size: 24),
              ),
              const SizedBox(width: 15),
              Text(title,
                  style: const TextStyle(
                      fontSize: 18,
                      fontWeight: FontWeight.w700,
                      color: Colors.black87)),
            ],
          ),
          const Divider(height: 30, thickness: 1, color: Color(0xFFF5F7FA)),
          ...children.map((e) => Padding(
                padding: const EdgeInsets.only(bottom: 16),
                child: e,
              )),
        ],
      ),
    );
  }

  Widget _buildSubmitButton() {
    return InkWell(
      onTap: _submitForm,
      borderRadius: BorderRadius.circular(18),
      child: Container(
        width: double.infinity,
        padding: const EdgeInsets.symmetric(vertical: 18),
        decoration: BoxDecoration(
          gradient: AppColors.primaryGradient,
          borderRadius: BorderRadius.circular(18),
        ),
        child: const Center(
          child: Text(
            "Save Information",
            style: TextStyle(
              color: Colors.white,
              fontWeight: FontWeight.w700,
              fontSize: 18,
            ),
          ),
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF5F7FA),
      appBar: AppBar(
        automaticallyImplyLeading: false,
        title: const Text(
          "Nurse Profile",
          style: TextStyle(fontWeight: FontWeight.w700, fontSize: 24),
        ),
        flexibleSpace: Container(
          decoration: const BoxDecoration(gradient: AppColors.primaryGradient),
        ),
        foregroundColor: Colors.white,
        elevation: 0,
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 25),
        child: Form(
          key: _formKey,
          child: Column(
            children: [
              _buildSection(
                title: "Personal Info",
                icon: Icons.person_rounded,
                children: [
                  _animatedTextField(_firstNameController, "First Name",
                      Icons.person_outline_rounded),
                  _animatedTextField(_middleNameController, "Middle Name",
                      Icons.person_outline_rounded,
                      requiredField: false),
                  _animatedTextField(_lastNameController, "Last Name",
                      Icons.person_outline_rounded),
                  _animatedTextField(_addressController, "Address",
                      Icons.home_outlined,
                      maxLines: 2),
                ],
              ),
              _buildSection(
                title: "Contact Info",
                icon: Icons.contact_page_rounded,
                children: [
                  _animatedTextField(_facebookController,
                      "Facebook Link (Optional)", Icons.facebook_rounded,
                      requiredField: false),
                  _animatedTextField(
                      _emailController, "Email", Icons.email_outlined,
                      keyboard: TextInputType.emailAddress),
                  _animatedTextField(_phoneController, "Phone Number",
                      Icons.phone_outlined,
                      keyboard: TextInputType.phone),
                ],
              ),
              _buildSection(
                title: "Security",
                icon: Icons.lock_rounded,
                children: [
                  _animatedTextField(
                    _passwordController,
                    "Password",
                    Icons.lock_outline_rounded,
                    isPassword: true,
                    passwordVisible: _passwordVisible,
                    togglePassword: () =>
                        setState(() => _passwordVisible = !_passwordVisible),
                  ),
                  _animatedTextField(
                    _rePasswordController,
                    "Re-enter Password",
                    Icons.lock_outline_rounded,
                    isPassword: true,
                    passwordVisible: _rePasswordVisible,
                    togglePassword: () =>
                        setState(() => _rePasswordVisible = !_rePasswordVisible),
                  ),
                ],
              ),
              const SizedBox(height: 30),
              _buildSubmitButton(),
            ],
          ),
        ),
      ),
    );
  }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Vbe.Interop;
using Microsoft.Vbe.Interop.Forms;
using Moq;

namespace RubberduckTests.Mocks
{
    public class MockUserFormBuilder
    {
        private readonly Mock<VBComponent> _component;
        private readonly Mock<Controls> _vbControls;
        private readonly ICollection<Mock<Control>> _controls = new List<Mock<Control>>();

        public MockUserFormBuilder(Mock<VBComponent> component)
        {
            if (component.Object.Type != vbext_ComponentType.vbext_ct_MSForm)
            {
                throw new InvalidOperationException("Component type must be 'vbext_ComponentType.vbext_ct_MSForm'.");
            }

            _component = component;
            _vbControls = CreateControlsMock();
        }

        public MockUserFormBuilder AddControl(string name)
        {
            var control = new Mock<Control>();
            control.SetupProperty(m => m.Name, name);

            _controls.Add(control);
            return this;
        }

        public Mock<VBComponent> Build()
        {
            var designer = CreateMockDesigner();
            _component.SetupGet(m => m.Designer).Returns(() => designer);

            return _component;
        }

        private Mock<UserForm> CreateMockDesigner()
        {
            var result = new Mock<UserForm>();
            result.SetupGet(m => m.Controls).Returns(() => _vbControls.Object);

            return result;
        }

        private Mock<Controls> CreateControlsMock()
        {
            var result = new Mock<Controls>();
            result.Setup(m => m.GetEnumerator()).Returns(() => _controls.GetEnumerator());
            result.As<IEnumerable>().Setup(m => m.GetEnumerator()).Returns(() => _controls.GetEnumerator());

            result.Setup(m => m.Item(It.IsAny<int>())).Returns<int>(index => _controls.ElementAt(index).Object);
            result.SetupGet(m => m.Count).Returns(_controls.Count);
            return result;
        }
    }
}
﻿var ClientSideArrayOp = (function (_super) {
    __extends(ClientSideArrayOp, _super);
    function ClientSideArrayOp() {
        return _super !== null && _super.apply(this, arguments) || this;
    }

    ClientSideArrayOp.prototype.returnToParam = function (OutParamaterName, data) {
        if (OutParamaterName && OutParamaterName != "") {
            Forguncy.CommandHelper.setVariableValue(OutParamaterName, data);
        } else {
            this.log("OutParamaterName was not set, the value is: " + JSON.stringify(data));
        }
    };

    ClientSideArrayOp.prototype.execute = function () {
        var params = this.CommandParam;
        var Operation = params.Operation;
        var inP = this.evaluateFormula(params.InParamaterName);
        var paramA = this.evaluateFormula(params.OperationParamaterAName);
        var paramB = this.evaluateFormula(params.OperationParamaterBName);
        var OutParamaterName = params.OutParamaterName;

        switch (Operation) {
            case SupportedOperations.Create: {
                this.returnToParam(OutParamaterName, []);

                break;
            }
            case SupportedOperations.Set: {

                if (!Array.isArray(inP)) {

                    this.log("Paramater [" + params.InParamaterName + "] should be an Array.");
                    return;
                }

                if (isNaN(paramA)) {
                    this.log("Paramater [" + params.OperationParamaterAName + "] should be a number.");
                    return;
                }

                inP[paramA] = paramB;
                this.returnToParam(OutParamaterName, inP);
                break;
            }
            case SupportedOperations.Get: {

                if (!Array.isArray(inP)) {
                    this.log("Paramater [" + params.InParamaterName + "] should be an Array.");
                    return;
                }

                if (isNaN(paramA)) {
                    this.log("Paramater [" + params.OperationParamaterAName + "] should be a number.");
                    return;
                }

                this.returnToParam(OutParamaterName, inP[paramA]);
                break;
            }
            case SupportedOperations.Length: {

                if (!Array.isArray(inP)) {
                    this.log("Paramater [" + params.InParamaterName + "] should be an Array.");
                    return;
                }

                this.returnToParam(OutParamaterName, inP.length);
                break;
            }
            case SupportedOperations.Push: {

                if (!Array.isArray(inP)) {
                    this.log("Paramater [" + params.InParamaterName + "] should be an Array.");
                    return;
                }

                inP.push(paramA);

                this.returnToParam(OutParamaterName, inP);
                break;
            }
            case SupportedOperations.Pop: {

                if (!Array.isArray(inP)) {
                    this.log("Paramater [" + params.InParamaterName + "] should be an Array.");
                    return;
                }

                var result = inP.pop();

                this.returnToParam(OutParamaterName, result);
                break;
            }
            case SupportedOperations.Unshift: {

                if (!Array.isArray(inP)) {
                    this.log("Paramater [" + params.InParamaterName + "] should be an Array.");
                    return;
                }

                inP.unshift(paramA);

                this.returnToParam(OutParamaterName, inP);
                break;
            }
            case SupportedOperations.Shift: {

                if (!Array.isArray(inP)) {
                    this.log("Paramater [" + params.InParamaterName + "] should be an Array.");
                    return;
                }

                var result = inP.shift();

                this.returnToParam(OutParamaterName, result);
                break;
            }
            case SupportedOperations.Concat: {

                if (!Array.isArray(inP)) {
                    this.log("Paramater [" + params.InParamaterName + "] should be an Array.");
                    return;
                }

                if (!Array.isArray(paramA)) {
                    this.log("Paramater [" + params.OperationParamaterAName + "] should be an Array.");
                    return;
                }

                this.log("提示：Concat操作不会对两个数组产生影响，而是生成一个新数组。如需将一个数组加入另一个数组中，请使用InsertRange操作。");
                var result = inP.concat(paramA);

                this.returnToParam(OutParamaterName, result);
                break;
            }
            case SupportedOperations.Slice: {

                if (!Array.isArray(inP)) {
                    this.log("Paramater [" + params.InParamaterName + "] should be an Array.");
                    return;
                }

                if (isNaN(paramA)) {
                    this.log("Paramater [" + params.OperationParamaterAName + "] should be a number.");
                    return;
                }

                if (isNaN(paramB)) {
                    this.log("Paramater [" + params.OperationParamaterBName + "] should be a number.");
                    return;
                }

                var result = inP.slice(paramA, paramB);

                this.returnToParam(OutParamaterName, result);
                break;
            }
            case SupportedOperations.InsertRange: {

                if (!Array.isArray(inP)) {
                    this.log("Paramater [" + params.InParamaterName + "] should be an Array.");
                    return;
                }

                if (!Array.isArray(paramA)) {
                    this.log("Paramater [" + params.OperationParamaterAName + "] should be an Array.");
                    return;
                }

                if (isNaN(paramB)) {
                    this.log("Paramater [" + params.OperationParamaterBName + "] should be a number.");
                    return;
                }

                var current = 0;
                paramA.forEach((item) => {
                    inP.splice(paramB + current, 0, item);
                    current++;
                });

                this.returnToParam(OutParamaterName, inP);
                break;
            }
            case SupportedOperations.RemoveRange: {

                if (!Array.isArray(inP)) {
                    this.log("Paramater [" + params.InParamaterName + "] should be an Array.");
                    return;
                }

                if (isNaN(paramA)) {
                    this.log("Paramater [" + params.OperationParamaterAName + "] should be a number.");
                    return;
                }


                if (isNaN(paramB)) {
                    this.log("Paramater [" + params.OperationParamaterBName + "] should be a number.");
                    return;
                }

                var removedItem = inP.splice(paramA, paramB);

                this.returnToParam(OutParamaterName, removedItem);
                break;
            }
            case SupportedOperations.IndexOf: {

                if (!Array.isArray(inP)) {
                    this.log("Paramater [" + params.InParamaterName + "] should be an Array.");
                    return;
                }

                var index = inP.indexOf(paramA);

                this.returnToParam(OutParamaterName, index);
                break;
            }
            case SupportedOperations.LastIndexOf: {

                if (!Array.isArray(inP)) {
                    this.log("Paramater [" + params.InParamaterName + "] should be an Array.");
                    return;
                }

                var index = inP.lastIndexOf(paramA);

                this.returnToParam(OutParamaterName, index);
                break;
            } case SupportedOperations.FormJSON: {
                var arr = JSON.parse(paramA);
                if (arr && Array.isArray(arr)) {
                    this.returnToParam(OutParamaterName, arr);
                } else {
                    this.log("Paramater [" + params.OperationParamaterAName + "] should be a JSON string of Array.");
                }

                break;
            }
            case SupportedOperations.ToJSON: {

                if (!Array.isArray(inP)) {
                    this.log("Paramater [" + params.InParamaterName + "] should be an Array.");
                    return;
                }

                this.returnToParam(OutParamaterName, JSON.stringify(inP));
                break;
            }
        }

    };

    var SupportedOperations = {
        Create: 0,
        Set: 1,
        Get: 2,
        Length: 3,
        Push: 4,
        Pop: 5,
        Unshift: 6,
        Shift: 7,
        Concat: 8,
        Slice: 9,
        InsertRange: 10,
        RemoveRange: 11,
        IndexOf: 12,
        LastIndexOf: 13,
        FromJSON: 14,
        ToJSON: 15
    }

    return ClientSideArrayOp;
}(Forguncy.CommandBase));

// Key format is "Namespace.ClassName, AssemblyName"
Forguncy.CommandFactory.registerCommand("CollectionOperationKit.ClientSideArrayOp, CollectionOperationKit", ClientSideArrayOp);


var ClientSideStringMapOp = (function (_super) {
    __extends(ClientSideStringMapOp, _super);
    function ClientSideStringMapOp() {
        return _super !== null && _super.apply(this, arguments) || this;
    }

    ClientSideStringMapOp.prototype.returnToParam = function (OutParamaterName, data) {
        if (OutParamaterName && OutParamaterName != "") {
            Forguncy.CommandHelper.setVariableValue(OutParamaterName, data);
        } else {
            this.log("OutParamaterName was not set, the value is: " + JSON.stringify(data));
        }
    };

    ClientSideStringMapOp.prototype.execute = function () {
        var params = this.CommandParam;
        var Operation = params.Operation;
        var inP = this.evaluateFormula(params.InParamater);
        var pKey = this.evaluateFormula(params.OperationParamaterKey);
        var pValue = this.evaluateFormula(params.OperationParamaterValue);
        var OutParamaterName = params.OutParamaterName;

        switch (Operation) {
            case SupportedOperations.Create: {
                this.returnToParam(OutParamaterName, new Map());
                break;
            }
            case SupportedOperations.Clear: {
                if (!inP instanceof Map) {
                    this.log("Paramater [" + params.InParamater + "] should be an Map.");
                    return;
                }

                inP.clear();
                this.returnToParam(OutParamaterName, inP);
                break;
            }
            case SupportedOperations.Set: {
                if (!inP instanceof Map) {
                    this.log("Paramater [" + params.InParamater + "] should be an Map.");
                    return;
                }

                inP.set(pKey, pValue);
                this.returnToParam(OutParamaterName, inP);
                break;
            }
            case SupportedOperations.Has: {
                if (!inP instanceof Map) {
                    this.log("Paramater [" + params.InParamater + "] should be an Map.");
                    return;
                }

                this.returnToParam(OutParamaterName, inP.has(pKey) ? 1 : 0);
                break;
            }
            case SupportedOperations.Get: {
                if (!inP instanceof Map) {
                    this.log("Paramater [" + params.InParamater + "] should be an Map.");
                    return;
                }

                this.returnToParam(OutParamaterName, inP.has(pKey) ? inP.get(pKey) : pValue);
                break;
            }
            case SupportedOperations.Size: {
                if (!inP instanceof Map) {
                    this.log("Paramater [" + params.InParamater + "] should be an Map.");
                    return;
                }

                this.returnToParam(OutParamaterName, inP.size);
                break;
            }
            case SupportedOperations.Delete: {
                if (!inP instanceof Map) {
                    this.log("Paramater [" + params.InParamater + "] should be an Map.");
                    return;
                }

                inP.delete(pKey);

                this.returnToParam(OutParamaterName, inP);
                break;
            }
            case SupportedOperations.Keys: {
                if (!inP instanceof Map) {
                    this.log("Paramater [" + params.InParamater + "] should be an Map.");
                    return;
                }

                this.returnToParam(OutParamaterName, Array.from(inP.keys()));
                break;
            }
            case SupportedOperations.Values: {
                if (!inP instanceof Map) {
                    this.log("Paramater [" + params.InParamater + "] should be an Map.");
                    return;
                }

                this.returnToParam(OutParamaterName, Array.from(inP.values()));
                break;
            }

        }

    };

    var SupportedOperations = {
        Create: 0,
        Get: 1,
        Set: 2,
        Has: 3,
        Delete: 4,
        Clear: 5,
        Size: 6,
        Keys: 7,
        Values: 8
    }

    return ClientSideStringMapOp;
}(Forguncy.CommandBase));

// Key format is "Namespace.ClassName, AssemblyName"
Forguncy.CommandFactory.registerCommand("CollectionOperationKit.ClientSideStringMapOp, CollectionOperationKit", ClientSideStringMapOp);

var ClientSideObjectOp = (function (_super) {
    __extends(ClientSideObjectOp, _super);
    function ClientSideObjectOp() {
        return _super !== null && _super.apply(this, arguments) || this;
    }

    ClientSideObjectOp.prototype.returnToParam = function (OutParamaterName, data) {
        if (OutParamaterName && OutParamaterName != "") {
            Forguncy.CommandHelper.setVariableValue(OutParamaterName, data);
        } else {
            this.log("OutParamaterName was not set, the value is: " + JSON.stringify(data));
        }
    };

    ClientSideObjectOp.prototype.execute = function () {
        var params = this.CommandParam;
        var Operation = params.Operation;
        var inP = this.evaluateFormula(params.InParamater);
        var pName = this.evaluateFormula(params.OperationParamaterName);
        var pValue = this.evaluateFormula(params.OperationParamaterValue);
        var OutParamaterName = params.OutParamaterName;

        switch (Operation) {
            case SupportedOperations.Create: {
                this.returnToParam(OutParamaterName, new Object());
                break;
            }
            case SupportedOperations.Null: {
                if (!inP instanceof Object) {
                    this.log("Paramater [" + params.InParamater + "] should be an Object.");
                    return;
                }

                inP = null;
                this.returnToParam(OutParamaterName, inP);
                break;
            }
            case SupportedOperations.Properties: {
                if (!inP instanceof Object) {
                    this.log("Paramater [" + params.InParamater + "] should be an Object.");
                    return;
                }

                var pops = [];

                for (pop in inP) {
                    pops.push(pop);
                }

                this.returnToParam(OutParamaterName, pops);
                break;
            }
            case SupportedOperations.GetPropertyValue: {
                if (!inP instanceof Object) {
                    this.log("Paramater [" + params.InParamater + "] should be an Object.");
                    return;
                }

                for (pop in inP) {
                    if (pop === pName) {
                        this.returnToParam(OutParamaterName, inP[pop]);
                        return;
                    }
                }

                this.returnToParam(OutParamaterName, pValue);
                break;
            }
            case SupportedOperations.SetPropertyValue: {
                if (!inP instanceof Object) {
                    this.log("Paramater [" + params.InParamater + "] should be an Object.");
                    return;
                }

                inP[pName] = pValue;

                this.returnToParam(OutParamaterName, inP);
                break;
            }
        }

    };

    var SupportedOperations = {
        Create: 0,
        Properties: 1,
        GetPropertyValue: 2,
        SetPropertyValue: 3,
        Null: 4
    }

    return ClientSideObjectOp;
}(Forguncy.CommandBase));

// Key format is "Namespace.ClassName, AssemblyName"
Forguncy.CommandFactory.registerCommand("CollectionOperationKit.ClientSideObjectOp, CollectionOperationKit", ClientSideObjectOp);
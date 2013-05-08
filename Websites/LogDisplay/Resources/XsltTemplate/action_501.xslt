<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_custoverrideupdfi">
        <xsl:choose>
            <xsl:when test="@custid">
                <table class="tblDesc">
                    <tr>
                        <!--<td class="colW1">
                            {lbl:custID}
                        </td>
                        <td class="colW2">
                            <xsl:value-of select="@custid" />
                        </td>-->
                        <td class="colW1">
                            {lbl:checkPermission}
                        </td>
                        <td class="colW2">
                            <xsl:value-of select="@checkpermission" />
                        </td>
                        <td class="colW1">
                            {lbl:updMinPos}
                        </td>
                        <td class="colW2">
                            <xsl:value-of select="@updminpos" />
                        </td>
                        <td class="colW1">
                            {lbl:updMaxPos}
                        </td>
                        <td class="colW2">
                            <xsl:value-of select="@updmaxpos" />
                        </td>
                    </tr>

                    <tr>

                        <td class="colW1">
                            {lbl:language}
                        </td>
                        <td class="colW2">
                            <xsl:value-of select="@language" />
                        </td>
                        <!--<td class="colW1">
              {lbl:userID}
            </td>
            <td class="colW2">
              <xsl:value-of select="@userid" />
            </td>-->
                        <!--<xsl:choose>
                            <xsl:when test="$IsInternal = 0">
                                <td class="colW1"></td>
                                <td class="colW2">

                                </td>
                            </xsl:when>
                            <xsl:otherwise>
                                <td class="colW1">{lbl:userID}</td>
                                <td class="colW2">

                                    <xsl:value-of select="@userid" />
                                </td>

                            </xsl:otherwise>
                        </xsl:choose>-->

                        <td class="colW1">
                            {lbl:subAccName}
                        </td>
                        <td class="colW2">
                            <xsl:value-of select="@subaccname" />
                        </td>
                        <td class="colW1">
                            {lbl:writeLog}
                        </td>
                        <td class="colW2">
                            <xsl:value-of select="@writelog" />
                        </td>
                    </tr>

                    <tr>
                        <td class="colW1">
                            {lbl:ip}
                        </td>
                        <td class="colW2">
                            <xsl:value-of select="@ip" />
                        </td>

                        <td class="colW1">
                            {lbl:errCode}
                        </td>
                        <td class="colW2">
                            <xsl:value-of select="@errcode" />
                        </td>
                        <td class="colW1">
                            {lbl:errMsg}
                        </td>
                        <td class="colW2">
                            <xsl:value-of select="@errmsg" />
                        </td>
                    </tr>
                    <tr>
                        <td class="colW1">
                            {lbl:minV201PT_2}
                        </td>
                        <td class="colW2">
                            <xsl:value-of select="@minv201pt_2" />
                        </td>

                        <td class="colW1">
                            {lbl:minV201PT_3}
                        </td>
                        <td class="colW2">
                            <xsl:value-of select="@minv201pt_3" />
                        </td>
                        <td class="colW1">
                            {lbl:maxV201PT_2}
                        </td>
                        <td class="colW2">
                            <xsl:value-of select="@maxv201pt_2" />
                        </td>
                    </tr>

                    <tr>

                        <td class="colW1">
                            {lbl:maxV201PT_3}
                        </td>
                        <td class="colW2">
                            <xsl:value-of select="@maxv201pt_3" />
                        </td>
                        <td class="colTitle"></td>
                        <td class="colValue">

                        </td>
                        <td class="colTitle"></td>
                        <td class="colValue">

                        </td>
                    </tr>

                </table>
            </xsl:when>
            <xsl:otherwise>
                <table class="tblDesc">
                    <xsl:for-each select="info">
                        <tr class="headerDetailNormal">
                            <td colspan="6">{lbl:custInfo}</td>
                        </tr>
                        <tr >
                            <!--<td class="colW1">
                                {lbl:custID}
                            </td>
                            <td class="colW2">
                                <xsl:value-of select="@custid" />
                            </td>-->
                            <td class="colW1">
                                {lbl:checkPermission}
                            </td>
                            <td class="colW2">
                                <xsl:value-of select="@checkpermission" />
                            </td>
                            <td class="colW1">
                                {lbl:updMinPos}
                            </td>
                            <td class="colW2">
                                <xsl:value-of select="@updminpos" />
                            </td>

                            <td class="colW1">
                                {lbl:updMaxPos}
                            </td>
                            <td class="colW2">
                                <xsl:value-of select="@updmaxpos" />
                            </td>
                        </tr>

                        <tr >
                            <td class="colW1">
                                {lbl:language}
                            </td>
                            <td class="colW2">
                                <xsl:value-of select="@language" />
                            </td>
                            <!--<td class="colW1">
                {lbl:userID}
              </td>
              <td class="colW2">
                <xsl:value-of select="@userid" />
              </td>-->
                            <!--<xsl:choose>
                                <xsl:when test="$IsInternal = 0">
                                    <td class="colW1"></td>
                                    <td class="colW2">

                                    </td>
                                </xsl:when>
                                <xsl:otherwise>
                                    <td class="colW1">{lbl:userID}</td>
                                    <td class="colW2">

                                        <xsl:value-of select="@userid" />
                                    </td>

                                </xsl:otherwise>
                            </xsl:choose>-->

                            <td class="colW1">
                                {lbl:subAccName}
                            </td>
                            <td class="colW2">
                                <xsl:value-of select="@subaccname" />
                            </td>
                            <td class="colW1">
                                {lbl:writeLog}
                            </td>
                            <td class="colW2">
                                <xsl:value-of select="@writelog" />
                            </td>
                        </tr>
                        <tr >

                            <td class="colW1">
                                {lbl:ip}
                            </td>
                            <td class="colW2">
                                <xsl:value-of select="@ip" />
                            </td>

                            <td class="colW1">
                                {lbl:errCode}
                            </td>
                            <td class="colW2">
                                <xsl:value-of select="@errcode" />
                            </td>
                            <td class="colW1">
                                {lbl:errMsg}
                            </td>
                            <td class="colW2">
                                <xsl:value-of select="@errmsg" />
                            </td>

                        </tr>
                    </xsl:for-each>
                    <xsl:for-each select="minmaxpt">
                        <tr class="headerDetailNormal">
                            <td colspan="6">{lbl:minMaxPT}</td>
                        </tr>
                        <tr >
                            <td class="colW1">
                                {lbl:minV201PT_2}
                            </td>
                            <td class="colW2">
                                <xsl:value-of select="@minv201pt_2" />
                            </td>
                            <td class="colW1">
                                {lbl:minV201PT_3}
                            </td>
                            <td class="colW2">
                                <xsl:value-of select="@minv201pt_3" />
                            </td>
                            <td class="colW1">
                                {lbl:maxV201PT_2}
                            </td>
                            <td class="colW2">
                                <xsl:value-of select="@maxv201pt_2" />
                            </td>
                        </tr>
                        <tr >
                            <td class="colW1">
                                {lbl:maxV201PT_3}
                            </td>
                            <td class="colW2">
                                <xsl:value-of select="@maxv201pt_3" />
                            </td>
                            <td class="colTitle"></td>
                            <td class="colValue">

                            </td>
                            <td class="colTitle"></td>
                            <td class="colValue">

                            </td>
                        </tr>
                    </xsl:for-each>
                </table>
            </xsl:otherwise>
        </xsl:choose>

    </xsl:template>



</xsl:stylesheet>
